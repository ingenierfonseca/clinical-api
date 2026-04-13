using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Service.Services
{
    public class PaymentService
    {
        private readonly IPaymentRepository _paymentRepo;
        private readonly IInvoiceRepository _invoiceRepo;
        private readonly ICustomerRepository _customerRepo;
        //private readonly IPaymentTypeRepository _paymentTypeRepo;
        private readonly ApplicationDbContext _context; // Necesario para la transacción

        public PaymentService(
            IPaymentRepository paymentRepo,
            IInvoiceRepository invoiceRepo,
            ICustomerRepository customerRepo,
            //IPaymentTypeRepository paymentTypeRepo,
            ApplicationDbContext context)
        {
            _paymentRepo = paymentRepo;
            _invoiceRepo = invoiceRepo;
            _customerRepo = customerRepo;
            //_paymentTypeRepo = paymentTypeRepo;
            _context = context;
        }

        public async Task<Result<Payment>> CreatePaymentAsync(Payment payment)
        {
            // Asegúrate que GetByIdAsync traiga los Payments incluidos (.Include(i => i.Payments))
            var invoice = await _invoiceRepo.GetByIdAsync(payment.InvoiceId);

            if (invoice == null)
                return Result<Payment>.Failure("La factura no existe o no pertenece a esta clínica.");

            if (invoice.CustomerId != payment.CustomerId)
                return Result<Payment>.Failure("La factura no pertenece al cliente especificado.");

            //if (!await _paymentTypeRepo.IsValidPaymentTypeAsync(payment.PaymentTypeId))
                //return Result<Payment>.Failure("El tipo de pago no es válido.");

            // 2. Lógica de montos
            decimal saldoPendiente = invoice.Total - invoice.Payments.Sum(p => p.Amount);

            if (payment.Amount > saldoPendiente)
                return Result<Payment>.Failure($"El monto excede el saldo pendiente ({saldoPendiente}).");

            if (payment.Date == DateTime.MinValue)
                payment.Date = DateTime.Now;

            // 3. Ejecución transaccional
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Guardar el pago
                var result = await _paymentRepo.AddAsync(payment);

                // Actualizar estado de factura
                if ((saldoPendiente - payment.Amount) <= 0)
                    invoice.StatusId = 2; // Pagado
                else
                    invoice.StatusId = 5; // Pago Parcial

                await _invoiceRepo.UpdateAsync(invoice);

                await transaction.CommitAsync();
                return Result<Payment>.Success(result);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return Result<Payment>.Failure("Ocurrió un error inesperado al procesar el pago.");
            }
        }
    }
}
