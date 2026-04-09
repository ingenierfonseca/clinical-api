using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class PaymentRepository: BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Result<ResponsePaymentDto>> CreatePaymentAsync(Payment payment)
        {
            if (!await new CustomerRepository(_context).IsValidPatientAsync(payment.CustomerId))
            {
                return Result<ResponsePaymentDto>.Failure("El paciente no existe o no pertenece a esta clínica.");
            }

            var invoice = await new InvoiceRepository(_context).GetByIdAsync(payment.InvoiceId);

            if (invoice == null)
            {
                return Result<ResponsePaymentDto>.Failure("La factura no existe o no pertenece a esta clínica.");
            }

            if (invoice.CustomerId != payment.CustomerId)
            {
                return Result<ResponsePaymentDto>.Failure("La factura no pertenece al CustomerId.");
            }

            if (!await new PaymentTypeRepository(_context).IsValidPaymentTypeAsync(payment.PaymentTypeId))
            {
                return Result<ResponsePaymentDto>.Failure("El tipo de pago no existe o no pertenece a esta clínica.");
            }

            decimal saldoPendiente = invoice.Total - invoice.Payments.Sum(p => p.Amount);
            if (payment.Amount > saldoPendiente)
            {
                return Result<ResponsePaymentDto>.Failure($"El monto excede el saldo pendiente ${saldoPendiente}.");
            }

            if (payment.Date == DateTime.MinValue)
            {
                payment.Date = DateTime.Now;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var result = await AddAsync(payment);

                if ((saldoPendiente - payment.Amount) <= 0)
                    invoice.StatusId = 2; // "Pagado"
                else
                    invoice.StatusId = 5; // "Pago Parcial"

                await new InvoiceRepository(_context).UpdateAsync(invoice);

                await transaction.CommitAsync();

                return Result<ResponsePaymentDto>.Success(ResponsePaymentDto.ToDto(result));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //_logger.LogError(ex, "Error al crear factura");
                throw new Exception("No se pudo procesar la factura. Intente de nuevo.");
            }
        }
    }
}
