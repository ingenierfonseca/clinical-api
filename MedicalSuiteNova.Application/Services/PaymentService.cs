using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PaymentService: BaseService<Payment>, IPaymentService
    {
        public PaymentService(IUnitOfWork uow, IMapper mapper): base(uow, mapper, uow.Payments) {}

        public async Task<Result<Payment>> CreatePaymentAsync(Payment payment)
        {
            var invoice = await _uow.Invoices.FirstOrDefaultAsync(
                i => i.Id == payment.InvoiceId,
                i => i.Payments
            );
            

            if (invoice == null)
                return Result<Payment>.Failure("La factura no existe o no pertenece a esta clínica.");

            if (invoice.CustomerId != payment.CustomerId)
                return Result<Payment>.Failure("La factura no pertenece al cliente especificado.");

            if (!await _uow.PaymentTypes.IsValidPaymentTypeAsync(payment.PaymentTypeId))
                return Result<Payment>.Failure("El tipo de pago no es válido.");

            decimal saldoPendiente = invoice.Total - invoice.Payments.Sum(p => p.Amount);

            if (payment.Amount > saldoPendiente)
                return Result<Payment>.Failure($"El monto excede el saldo pendiente ({saldoPendiente}).");

            if (payment.Date == DateTime.MinValue)
                payment.Date = DateTime.Now;

            try
            {
                await _uow.BeginTransactionAsync();
                var result = await _uow.Payments.AddAsync(payment);

                // Actualizar estado de factura
                if (saldoPendiente - payment.Amount <= 0)
                    invoice.StatusId = (int)InvoiceStatusEnum.Pagada;
                else
                {
                    if (invoice.IssueDate < DateTime.Now)
                        invoice.StatusId = (int)InvoiceStatusEnum.Vencida;
                    else
                        invoice.StatusId = (int)InvoiceStatusEnum.PagoParcial;
                }

                await _uow.Invoices.UpdateAsync(invoice);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<Payment>.Success(result);
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                return Result<Payment>.Failure("Ocurrió un error inesperado al procesar el pago.");
            }
        }
    }
}
