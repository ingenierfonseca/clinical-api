using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class PaymentService(IUnitOfWork uow, IMapper mapper) : BaseService<Payment>(uow, mapper, uow.Payments), IPaymentService
    {
        public async Task<Result<PaymentDto>> CreatePaymentAsync(PaymentDto dto)
        {
            var invoice = await _uow.Invoices.FirstOrDefaultAsync(
                i => i.Id == dto.InvoiceId,
                i => i.Payments
            );
            

            if (invoice == null)
                return Result<PaymentDto>.Failure("La factura no existe o no pertenece a esta clínica.");

            if (invoice.CustomerId != dto.CustomerId)
                return Result<PaymentDto>.Failure("La factura no pertenece al cliente especificado.");

            if (!await _uow.PaymentTypes.IsValidPaymentTypeAsync(dto.PaymentTypeId))
                return Result<PaymentDto>.Failure("El tipo de pago no es válido.");

            decimal saldoPendiente = invoice.Total - invoice.Payments.Sum(p => p.Amount);

            if (dto.Amount > saldoPendiente)
                return Result<PaymentDto>.Failure($"El monto excede el saldo pendiente ({saldoPendiente}).");

            if (dto.Date == DateTime.MinValue)
                dto.Date = DateTime.Now;

            try
            {
                var payment = _mapper.Map<Payment>(dto);
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

                return Result<PaymentDto>.Success(_mapper.Map<PaymentDto>(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                return Result<PaymentDto>.Failure("Ocurrió un error inesperado al procesar el pago.");
            }
        }
    }
}
