
using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class SessionPlanMasterService(IUnitOfWork uow, IMapper mapper) : BaseService<SessionPlanMaster>(uow, mapper, uow.SessionPlanMaster), ISessionPlanMasterService
    {
        public async Task<Result<SessionPlanMasterDto>> AddAsync(RequestSessionPlanMaster request)
        {
            var clinicalSession = await _uow.ClinicalSessions.FindAsync(request.SessionId);
            if (clinicalSession == null)
                return Result<SessionPlanMasterDto>.Failure("El SessionId no es válido.");

            if (!await _uow.Currencies.ExistsAsync(request.CurrencyId))
                return Result<SessionPlanMasterDto>.Failure("El CurrencyId no es válido.");

            if (request.PlansIds == null || request.PlansIds.Count == 0)
                return Result<SessionPlanMasterDto>.Failure("El detalle del plan es requerido.");

            var templates = await _uow.TreatmentPlanTemplates.GetAllAsync(t => request.PlansIds.Contains(t.Id), t => t.Items!);

            if (templates.Count != request.PlansIds.Count)
                return Result<SessionPlanMasterDto>.Failure("Una o más plantillas seleccionadas no son válidas.");

            SessionPlanMaster session = new()
            {
                SessionId = request.SessionId,
                Name = request.Name,
                CurrencyId = request.CurrencyId,
                TotalEstimatedPrice = 0,
                Status = PlanStatus.Pending,
                Comments = request.Comments,

                // Datos de financiamiento (vienen del request basado en lo que pidió la Dra)
                /*IsFinanced = request.IsFinanced,
                DownPayment = request.DownPayment,
                InstallmentAmount = request.InstallmentAmount,
                TotalInstallments = request.TotalInstallments,*/

                Items = []
            };

            foreach (var template in templates)
            {
                session.StartDate = DateTime.UtcNow;
                session.EndDate = session.StartDate.AddMonths(template.EstimatedDurationMonths);
                session.CurrencyId = template.CurrencyId;
                session.TotalEstimatedPrice += template.BasePrice;

                foreach (var detail in template.Items!)
                {
                    session.Items.Add(new SessionPlanDetail
                    {
                        TreatmentPlanTemplateItemId = detail.Id,
                        Status = PlanStatus.Pending
                    });
                }
            }

            await _uow.BeginTransactionAsync();
            try
            {
                var result = await _uow.SessionPlanMaster.AddAsync(session);
                await _uow.CompleteAsync();

                // LOGICA DEL LEDGER (Qué afectó en la cuenta)
                // Obtenemos el último saldo del paciente para calcular el nuevo
                var currentBalance = await _uow.Ledger.GetLastBalanceByCustomerIdAsync(clinicalSession.CustomerId);

                var ledgerEntry = new CustomerAccountLedger
                {
                    CustomerId = clinicalSession.CustomerId,
                    TransactionType = "CHARGE",
                    ReferenceId = session.Id,
                    ReferenceTable = "SessionPlanMaster",
                    Amount = session.TotalEstimatedPrice,
                    CurrencyId = session.CurrencyId,
                    //ExchangeRate = payment.ExchangeRate,
                    BalanceAfter = currentBalance - session.TotalEstimatedPrice,
                    Description = $"Aceptacion de plan de tratamiento {request.Name}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Test"
                };

                await _uow.Ledger.AddAsync(ledgerEntry);

                /*if (request.DownPayment > 0)
                {
                    var invoice = new Invoice
                    {
                        CustomerId = request.CustomerId,
                        IssueDate = DateTime.UtcNow,
                        CurrencyId = request.CurrencyId,
                        ExchangeRate = await _exchangeService.GetCurrentRate(),
                        TotalAmount = request.DownPayment,
                        Status = "Pagada", // Al ser prima inicial en el acto
                        Items = new List<InvoiceItem>
                        {
                            new()
                            {
                                Description = $"Prima inicial - Plan: {request.Name}",
                                Quantity = 1,
                                UnitPrice = request.DownPayment,
                                Subtotal = request.DownPayment
                            }
                        }
                    };

                    var savedInvoice = await _uow.Invoices.AddAsync(invoice);
                    await _uow.CompleteAsync();

                    // 5. REGISTRAR EN EL LEDGER (El "Gran Libro")
                    // Cargo: El paciente ahora debe la prima
                    await _uow.Ledger.AddAsync(new PatientAccountLedger
                    {
                        PatientId = request.PatientId,
                        TransactionType = "CHARGE",
                        ReferenceId = savedPlan.Id,
                        Amount = request.DownPayment,
                        Description = $"Cargo por prima inicial de plan"
                    });

                    // Abono: El paciente pagó la prima (referencia a la factura)
                    await _uow.Ledger.AddAsync(new PatientAccountLedger
                    {
                        PatientId = request.PatientId,
                        TransactionType = "PAYMENT",
                        ReferenceId = savedInvoice.Id,
                        Amount = request.DownPayment,
                        Description = $"Pago de prima inicial - Factura #{savedInvoice.Id}"
                    });

                    await _uow.CompleteAsync();
                }*/

                await _uow.CommitTransactionAsync();
                return Result<SessionPlanMasterDto>.Success(_mapper.Map<SessionPlanMasterDto>(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                return Result<SessionPlanMasterDto>.Failure("Ocurrió un error inesperado al procesar el plan.");
            }
        }
    }
}
