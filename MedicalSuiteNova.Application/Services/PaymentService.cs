using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PaymentService(IUnitOfWork uow, IMapper mapper, IInvoiceService invoiceService) : BaseService<Payment>(uow, mapper, uow.Payments), IPaymentService
    {
        public async Task<Result<PaymentDto>> CreatePaymentAsync(PaymentRequest request)
        {
            if (!await _uow.PaymentTypes.IsValidPaymentTypeAsync(request.PaymentTypeId))
                return Result<PaymentDto>.Failure("El tipo de pago no es válido.");

            var currentBalance = await _uow.Ledger.GetLastBalanceByCustomerIdAsync(request.CustomerId);

            // CORRECCIÓN DEL BUG: El monto no puede ser mayor a lo que debe actualmente
            if (request.Amount > currentBalance)
                return Result<PaymentDto>.Failure($"El monto (${request.Amount}) excede el saldo pendiente actual (${currentBalance}).");

            // Usamos el Change Tracker cargando al cliente para actualizar su saldo al final
            var customer = await _uow.Customers.FindAsync(request.CustomerId);
            if (customer == null)
                return Result<PaymentDto>.Failure("El cliente no existe o no pertenece a esta clínica.");

            Invoice? invoice;

            if (request.OperationTypeId == (int)OperationTypeEnum.PagoFactura)
            {
                invoice = await _uow.Invoices.FindAsync(request.InvoiceId);

                if (invoice == null)
                    return Result<PaymentDto>.Failure("La factura no existe o no pertenece a esta clínica.");

                if (invoice.CustomerId != request.CustomerId)
                    return Result<PaymentDto>.Failure("La factura no pertenece al cliente especificado.");
            }
            else if (request.OperationTypeId == (int)OperationTypeEnum.AbonoSaldo)
            {
                var invoiceNumber = await invoiceService.GenerateInvoiceNumberAsync();

                invoice = new Invoice
                {
                    CustomerId = request.CustomerId,
                    SubTotal = request.Amount,
                    Total = request.Amount,
                    Number = invoiceNumber,
                    IssueDate = DateTime.UtcNow,
                    DueDate = DateTime.UtcNow,
                    CurrencyId = request.CurrencyId,
                    StatusId = (int)InvoiceStatusEnum.Pendiente,
                    PaymentTermId = 1, // Pago inmediato
                    CreatedBy = "System"
                };

                await _uow.Invoices.AddAsync(invoice);
            }
            else
            {
                return Result<PaymentDto>.Failure("El tipo de transacción no está soportado.");
            }

            await _uow.BeginTransactionAsync();
            try
            {
                if (invoice.Id == 0)
                {
                    await _uow.CompleteAsync();
                }

                invoice.StatusId = (int)InvoiceStatusEnum.Pagada;
                await _uow.Invoices.UpdateAsync(invoice);

                var payment = new Payment
                {
                    InvoiceId = invoice.Id,
                    CustomerId = request.CustomerId,
                    CurrencyId = request.CurrencyId,
                    Amount = request.Amount,
                    PaymentTypeId = request.PaymentTypeId,
                    Date = request.Date == DateTime.MinValue ? DateTime.UtcNow : request.Date.ToUniversalTime()
                };
                var result = await _uow.Payments.AddAsync(payment);

                var ledgerEntry = new CustomerAccountLedger
                {
                    CustomerId = invoice.CustomerId,
                    TransactionType = "PAYMENT",
                    ReferenceId = invoice.Id,
                    ReferenceTable = "Invoice",
                    Amount = payment.Amount,
                    CurrencyId = invoice.CurrencyId,
                    BalanceAfter = currentBalance - payment.Amount,
                    Description = $"Abono a Factura #{invoice.Number} de forma exitosa.",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System"
                };
                await _uow.Ledger.AddAsync(ledgerEntry);

                customer.Balance = ledgerEntry.BalanceAfter;
                await _uow.Customers.UpdateAsync(customer);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<PaymentDto>.Success(_mapper.Map<PaymentDto>(result));
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                // Loguear internamente el error real para soporte técnico antes de enmascarar la respuesta
                Console.WriteLine($"[ClinicalSuiteNova Error]: {ex.Message} -> {ex.InnerException?.Message}");
                return Result<PaymentDto>.Failure("Ocurrió un error inesperado al procesar y asentar el pago.");
            }
        }
    }
}
