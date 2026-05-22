
using AutoMapper;
using MedicalSuiteNova.Application.Constants;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Validation;

namespace MedicalSuiteNova.Application.Services
{
    public class SessionPlanMasterService(IUnitOfWork uow, IMapper mapper, IInvoiceService invoiceService) : BaseService<SessionPlanMaster>(uow, mapper, uow.SessionPlanMaster), ISessionPlanMasterService
    {
        public async Task<Result<SessionPlanMasterDto>> AddAsync(RequestSessionPlanMaster request)
        {
            var validation = await ValidateAsync(request);
            if (!validation.IsSuccess)
                return Result<SessionPlanMasterDto>.Failure(validation.ErrorMessage);

            var clinicalSession = validation.Value.ClinicalSession;
            var customer = validation.Value.Customer;
            var templates = validation.Value.PlanTemplates;

            var session = CreateSessionPlan(request, customer.Id, templates);

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
                    TransactionType = LedgerConstants.CHARGE,
                    ReferenceId = session.Id,
                    ReferenceTable = LedgerConstants.TblSessionPlanMaster,
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
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                return Result<SessionPlanMasterDto>.Failure("Ocurrió un error inesperado al procesar el plan.");
            }
        }

        private async Task<Result<SessionPlanValidationContext>> ValidateAsync(RequestSessionPlanMaster request)
        {
            var clinicalSession = await _uow.ClinicalSessions.FirstOrDefaultAsync(c => c.Id == request.SessionId, c => c.Customer!);
            if (clinicalSession == null)
                return Result<SessionPlanValidationContext>.Failure("El SessionId no es válido.");

            if (!await _uow.Currencies.ExistsAsync(request.CurrencyId))
                return Result<SessionPlanValidationContext>.Failure("El CurrencyId no es válido.");

            if (!await _uow.PaymentTerms.ExistsAsync(request.PaymentTermId))
                return Result<SessionPlanValidationContext>.Failure("El PaymentTermId no es válido.");

            if (request.IsFinanced && request.DownPayment == 0)
                return Result<SessionPlanValidationContext>.Failure("El DownPaymentAmount es requerido.");

            if (request.PlansIds == null || request.PlansIds.Count == 0)
                return Result<SessionPlanValidationContext>.Failure("El detalle del plan es requerido.");

            var templates = await _uow.TreatmentPlanTemplates.GetAllAsync(t => request.PlansIds.Contains(t.Id), t => t.Items!);

            if (templates.Count != request.PlansIds.Count)
                return Result<SessionPlanValidationContext>.Failure("Una o más plantillas seleccionadas no son válidas.");

            return Result<SessionPlanValidationContext>.Success(new()
            {
                ClinicalSession = clinicalSession,
                PlanTemplates = templates,
                Customer = clinicalSession.Customer!
            });
        }

        private SessionPlanMaster CreateSessionPlan(RequestSessionPlanMaster request, int customerId, List<TreatmentPlanTemplate> templates)
        {
            SessionPlanMaster session = new()
            {
                SessionId = request.SessionId,
                CustomerId = customerId,
                Name = request.Name,
                CurrencyId = request.CurrencyId,
                PaymentTermId = request.PaymentTermId,
                TotalEstimatedPrice = 0,
                Status = PlanStatus.Pending,
                Comments = request.Comments,
                IsFinanced = request.IsFinanced,
                DownPayment = request.DownPayment,
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

            return session;
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

        public async Task<Result<decimal>> GetTotalPaidAsync(int id)
        {
            if (await _uow.SessionPlanMaster.ExistsAsync(id))
                return Result<decimal>.Failure("El Id no es válido.");

            var invoice = await _uow.Invoices.FirstOrDefaultAsync(i => i.SessionPlanMasterId == id, i => i.Payments!);
            
            if (invoice == null)
                return Result<decimal>.Failure("El Id no es válido.");

            return Result<decimal>.Success(invoice.Payments.Sum(p => p.Amount));
        }
    }
}
