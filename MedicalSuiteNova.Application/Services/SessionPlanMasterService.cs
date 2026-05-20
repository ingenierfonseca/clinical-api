
using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class SessionPlanMasterService(IUnitOfWork uow, IMapper mapper, IInvoiceService invoiceService) : BaseService<SessionPlanMaster>(uow, mapper, uow.SessionPlanMaster), ISessionPlanMasterService
    {
        public async Task<Result<SessionPlanMasterDto>> AddAsync(RequestSessionPlanMaster request)
        {
            var clinicalSession = await _uow.ClinicalSessions.FirstOrDefaultAsync(c => c.Id == request.SessionId, c => c.Customer!);
            if (clinicalSession == null)
                return Result<SessionPlanMasterDto>.Failure("El SessionId no es válido.");

            var customer = clinicalSession.Customer;
            if (customer == null)
                return Result<SessionPlanMasterDto>.Failure("El CustomerId no es válido.");

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
                CustomerId = clinicalSession.CustomerId,
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

                var invoice = await invoiceService.CreateBalanceInvoicePlanAsync(
                    session.Name, 
                    clinicalSession.CustomerId,
                    session.CurrencyId,
                    session.TotalEstimatedPrice
                );
                await _uow.Invoices.AddAsync(invoice);

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
                    BalanceAfter = currentBalance + session.TotalEstimatedPrice,
                    Description = $"Aceptacion de plan de tratamiento {request.Name}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "Test"
                };

                await _uow.Ledger.AddAsync(ledgerEntry);

                customer.CurrencyId = session.CurrencyId;
                customer.Balance = ledgerEntry.BalanceAfter;

                var visitEntry = new ClinicalVisits
                {
                    CustomerId = clinicalSession.CustomerId,
                    DoctorId = clinicalSession.DoctorId,
                    VisitDate = DateTime.UtcNow,
                    Notes = request.Comments
                };

                await _uow.ClinicalVisits.AddAsync(visitEntry);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();
                return Result<SessionPlanMasterDto>.Success(_mapper.Map<SessionPlanMasterDto>(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                return Result<SessionPlanMasterDto>.Failure("Ocurrió un error inesperado al procesar el plan.");
            }
        }

        public async Task<Result<SessionPlanMasterDto>> ChangeStatus(RequestStatusSessionPlanMaster request)
        {
            var session = await _uow.SessionPlanMaster.FirstOrDefaultAsync(s => s.Id == request.Id, s => s.Items!);
            if (session == null)
                return Result<SessionPlanMasterDto>.Failure("El Id no es válido.");
            
            var item = session.Items!.FirstOrDefault(i => i.Id == request.ItemId);
            if (item == null)
                return Result<SessionPlanMasterDto>.Failure("El ItemId no es válido.");

            if (!PlanStatus.AllStatus().Contains(request.Status))
                return Result<SessionPlanMasterDto>.Failure("El Status no es válido.");

            try
            {
                item.Status = request.Status;
                if (request.Status == PlanStatus.Completed)
                {
                    item.CompletedAt = DateTime.UtcNow;
                }

                bool allCompleted = session.Items!.All(i => i.Status == PlanStatus.Completed);
                bool allPending = session.Items!.All(i => i.Status == PlanStatus.Pending);

                if (allCompleted)
                    session.Status = PlanStatus.Completed;
                else if (allPending)
                    session.Status = PlanStatus.Pending;
                else
                    session.Status = PlanStatus.InProcess;
                
                await _uow.SessionPlanMaster.UpdateAsync(session);
                await _uow.CompleteAsync();

                return Result<SessionPlanMasterDto>.Success(_mapper.Map<SessionPlanMasterDto>(session));
            }
            catch (Exception)
            {
                return Result<SessionPlanMasterDto>.Failure("Ocurrió un error inesperado al cambiar el estado.");
            }
        }

        public async Task<List<SessionPlanMasterDto>> GetByCustomer(int id)
        {
            var data = await _uow.SessionPlanMaster.GetAllAsync(t => t.CustomerId == id);
            return _mapper.Map<List<SessionPlanMasterDto>>(data);
        }
    }
}
