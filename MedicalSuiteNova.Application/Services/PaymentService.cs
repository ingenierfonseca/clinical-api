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
    public class PaymentService(IUnitOfWork uow, IMapper mapper, IInvoiceService invoiceService) : BaseService<Payment>(uow, mapper, uow.Payments), IPaymentService
    {
        public async Task<Result<PaymentDto>> CreatePaymentAsync(PaymentRequest request)
        {
            var validation = await ValidateAsync(request);
            if (!validation.IsSuccess)
                return Result<PaymentDto>.Failure(validation.ErrorMessage);

            await _uow.BeginTransactionAsync();
            try
            {
                var validationBalance = await ValidateBalanceAsync(request);
                if (!validationBalance.IsSuccess)
                    return Result<PaymentDto>.Failure(validationBalance.ErrorMessage);

                var invoice = validationBalance.Value.Invoice;
                var customer = validationBalance.Value.Customer;
                var currentBalance = validationBalance.Value.CurrentBalance;

                await SaveInvoice(invoice, request.Amount);

                var payment = await CreatePaymentRecordAsync(request, invoice);

                await RegisterLedgerAsync(customer, invoice, payment, currentBalance);
                UpdateCustomerBalance(customer, currentBalance - payment.Amount);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<PaymentDto>.Success(_mapper.Map<PaymentDto>(payment));
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                // Loguear internamente el error real para soporte técnico antes de enmascarar la respuesta
                Console.WriteLine($"[ClinicalSuiteNova Error]: {ex.Message} -> {ex.InnerException?.Message}");
                return Result<PaymentDto>.Failure("Ocurrió un error inesperado al procesar y asentar el pago.");
            }
        }

        private async Task<Result<string>> ValidateAsync(PaymentRequest request)
        {
            if (!await _uow.PaymentTypes.ExistsAsync(request.PaymentTypeId))
                return Result<string>.Failure("El tipo de pago no es válido.");

            return Result<string>.Success("");
        }

        private async Task<Result<PaymentValidationContext>> ValidateBalanceAsync(PaymentRequest request)
        {
            var result = await ResolveInvoiceAsync(request);
            var invoice = result.Value;

            if (!result.IsSuccess)
                return Result<PaymentValidationContext>.Failure(result.ErrorMessage);

            if (invoice.CustomerId != request.CustomerId)
                return Result<PaymentValidationContext>.Failure("La factura no pertenece al cliente especificado.");

            var currentBalance = await _uow.Ledger.GetLastBalanceByCustomerIdAsync(request.CustomerId);

            if (invoice.Id == 0 && request.Amount > currentBalance)
                return Result<PaymentValidationContext>.Failure($"El monto (${request.Amount}) excede el saldo pendiente actual (${currentBalance}).");

            var currentInvoiceBalance = invoice.Total - invoice.Payments.Sum(p => p.Amount);

            if (result.Value.Id != 0 && request.Amount > currentInvoiceBalance)
                return Result<PaymentValidationContext>.Failure($"El monto (${request.Amount}) excede el saldo pendiente actual de la factura (${currentInvoiceBalance}).");

            var customer = await _uow.Customers.FindAsync(request.CustomerId);
            if (customer == null)
                return Result<PaymentValidationContext>.Failure("El cliente no existe o no pertenece a esta clínica.");

            return Result<PaymentValidationContext>.Success(new ()
            {
                Invoice = result.Value,
                Customer = customer,
                CurrentBalance = currentBalance
            });
        }

        private async Task<Result<Invoice>> ResolveInvoiceAsync(PaymentRequest request)
        {
            return request.OperationTypeId switch
            {
                (int)OperationTypeEnum.InvoicePayment =>
                    await ResolveExistingInvoiceAsync(request.InvoiceId),

                (int)OperationTypeEnum.BalancePayment =>
                    Result<Invoice>.Success(await CreateBalanceInvoiceAsync(request)),

                _ => Result<Invoice>.Failure("Operación no soportada")
            };
        }

        private async Task<Result<Invoice>> ResolveExistingInvoiceAsync(int invoiceId)
        {
            var invoice = await _uow.Invoices.FirstOrDefaultAsync(i => i.Id == invoiceId, i => i.Payments);

            if (invoice == null)
                return Result<Invoice>.Failure("Factura no encontrada.");

            return Result<Invoice>.Success(invoice);
        }

        private async Task SaveInvoice(Invoice invoice, decimal amount)
        {
            if (invoice.Id == 0)
            {
                await _uow.Invoices.AddAsync(invoice);
                await _uow.CompleteAsync();
                return;
            }

            var historicalPaidAmount = invoice.Payments.Sum(p => p.Amount);
            if (historicalPaidAmount + amount >= invoice.Total)
                invoice.StatusId = (int)InvoiceStatusEnum.Paid;
            else
                invoice.StatusId = (int)InvoiceStatusEnum.PartialPayment;

            await _uow.Invoices.UpdateAsync(invoice);
        }

        private async Task<Invoice> CreateBalanceInvoiceAsync(PaymentRequest request)
        {
            var invoiceNumber = await invoiceService.GenerateInvoiceNumberAsync();

            return new Invoice
            {
                CustomerId = request.CustomerId,
                SubTotal = request.Amount,
                Total = request.Amount,
                Number = invoiceNumber,
                IssueDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow,
                CurrencyId = request.CurrencyId,
                StatusId = (int)InvoiceStatusEnum.Pending,
                PaymentTermId = (int)PaymentTermEnum.Cash,
                CreatedBy = AuditConstants.CreatedBy,
                OriginType = "ABONO",
                Items = [
                    new ()
                        {
                            Description = $"Abono general para saldo pendiente",
                            Quantity = 1,
                            UnitPrice = request.Amount,
                            LineTotal = request.Amount,
                            OriginalCurrencyId = request.CurrencyId,
                            OriginalPrice = request.Amount
                        }
                ]
            };
        }

        private async Task<Payment> CreatePaymentRecordAsync(PaymentRequest request, Invoice invoice)
        {
            var payment = new Payment
            {
                InvoiceId = invoice.Id,
                CustomerId = request.CustomerId,
                CurrencyId = request.CurrencyId,
                Amount = request.Amount,
                PaymentTypeId = request.PaymentTypeId,
                Date = request.Date == DateTime.MinValue ? DateTime.UtcNow : request.Date.ToUniversalTime()
            };

            return await _uow.Payments.AddAsync(payment);
        }

        private async Task RegisterLedgerAsync(Customer customer, Invoice invoice, Payment payment, decimal currentBalance)
        {
            var ledger = new CustomerAccountLedger
            {
                CustomerId = customer.Id,
                ReferenceId = invoice.Id,
                ReferenceTable = LedgerConstants.TblInvoice,
                TransactionType = LedgerConstants.PAYMENT,
                CurrencyId = payment.CurrencyId,
                Amount = payment.Amount,
                BalanceAfter = currentBalance - payment.Amount,
                Description = $"Pago aplicado a factura #{invoice.Number}",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = AuditConstants.CreatedBy
            };

            await _uow.Ledger.AddAsync(ledger);
        }

        private static void UpdateCustomerBalance(Customer customer, decimal newBalance)
        {
            customer.Balance = newBalance;
        }
    }
}
