
using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Utils;
using Microsoft.VisualBasic;

namespace MedicalSuiteNova.Application.Services
{
    public class InvoiceService : BaseService<Invoice>, IInvoiceService
    {
        public InvoiceService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Invoices) { }

        public async Task<PagedResponse<InvoiceItemInfoDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _uow.Invoices.GetInvoicesAsQueryable();

            var pagedData = await _uow.Invoices.GetAllAsync(pageNumber, pageSize, query);

            return pagedData;
        }

        public async Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id)
        {
            return await _uow.Invoices.GetByIdDtoAsync(id);
        }

        public async Task<List<CustomerDashboardDto>> GetDashboard()
        {
            var dashboardList = new List<CustomerDashboardDto>();
            var now = DateTime.UtcNow;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddMonths(-1);

            var allInvoices = await _uow.Invoices.GetAllAsync();
            int totalCountPeding = allInvoices.Where(a => a.StatusId == (int)InvoiceStatusEnum.Pendiente || a.StatusId == (int)InvoiceStatusEnum.PagoParcial).Count();
            int totalCountPaid = allInvoices.Where(a => a.StatusId == 2).Count();

            dashboardList.Add(new CustomerDashboardDto
            {
                Title = "Facturas Pendientes",
                Value = totalCountPeding.ToString()
            });

            dashboardList.Add(new CustomerDashboardDto
            {
                Title = "Facturas Pagadas",
                Value = totalCountPaid.ToString()
            });

            return dashboardList;
        }

        public async Task<PagedResponse<CustomerInvoiceDashboardDto>> GetAllDashboardPaginatedAsync(int pageNumber, int pageSize, string search)
        {
            return await _uow.Invoices.GetAllDashboardPaginatedAsync(pageNumber, pageSize, search);
        }

        public async Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id)
        {
            return await _uow.Invoices.GetInvoicesByCustomerAsync(id);
        }

        public async Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id)
        {
            return await _uow.Invoices.GetPaymentsByCustomer(id);
        }

        public async Task<Result<ResponseInvoiceDto>> CreateInvoiceAsync(RequestInvoiceDto dto)
        {
            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<ResponseInvoiceDto>.Failure("El paciente no existe o no pertenece a esta clínica.");

            if (!await _uow.Currencies.ExistsAsync(dto.CurrencyId))
                return Result<ResponseInvoiceDto>.Failure("No se encontro el CurrencyId");

            var term = await _uow.PaymentTerms.FindAsync(dto.PaymentTermId);
            if (term == null)
                return Result<ResponseInvoiceDto>.Failure("No se encontro el PaymentTermId");

            var validateDate = DateTimeHelper.ValidateIssueDate(dto.IssueDate);
            if (!validateDate.isValid)
                return Result<ResponseInvoiceDto>.Failure(validateDate.message);

            DateTime dueDate = dto.IssueDate.AddDays(term.DaysToDue);

            try
            {
                await _uow.BeginTransactionAsync();
                var invoice = new Invoice
                {
                    CustomerId = dto.CustomerId,
                    CurrencyId = dto.CurrencyId,
                    StatusId = dto.StatusId,
                    PaymentTermId = dto.PaymentTermId,
                    Number = await GenerateInvoiceNumberAsync(),
                    IssueDate = dto.IssueDate,
                    DueDate = dueDate,
                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<InvoiceItem>()
                };

                decimal runningSubTotal = 0;
                decimal runningTax = 0;

                foreach (var itemDto in dto.Items)
                {
                    var treatment = await _uow.Treatments.FindAsync(x => x.Name == itemDto.Description);
                    if (treatment == null)
                        return Result<ResponseInvoiceDto>.Failure($"No se puede procesar la Factura el Tratamiento {itemDto.Description} no existe o no pertenece a esta clínica.");

                    //var tax = itemDto.Tax / 100;
                    //var lineTax = (treatment.Price * itemDto.Quantity * tax);
                    decimal effectivePrice = treatment.Price;
                    if (invoice.CurrencyId != treatment.CurrencyId)
                    {
                        var exchangeRate = await _uow.ExchangeRates.GetLatestRate(
                            fromCurrencyId: treatment.CurrencyId,
                            toCurrencyId: invoice.CurrencyId);

                        if (exchangeRate == 0)
                            throw new Exception("No se encontró un tipo de cambio configurado para esta conversión.");

                        effectivePrice = treatment.Price * exchangeRate;
                    }

                    var lineTotal = (effectivePrice * itemDto.Quantity) - itemDto.Tax;

                    var detail = new InvoiceItem
                    {
                        Description = itemDto.Description,
                        Quantity = itemDto.Quantity,
                        UnitPrice = effectivePrice,
                        TaxAmount = itemDto.Tax,
                        LineTotal = lineTotal
                    };

                    invoice.Items!.Add(detail);

                    runningSubTotal += effectivePrice * itemDto.Quantity;
                    runningTax += lineTotal;
                }

                invoice.SubTotal = runningSubTotal;
                invoice.TaxTotal = runningTax;
                invoice.Total = runningSubTotal + runningTax;

                var result = await AddAsync(invoice);

                await _uow.CommitTransactionAsync();

                return Result<ResponseInvoiceDto>.Success(ResponseInvoiceDto.ToDto(result));
            }
            catch (Exception ex)
            {
                await _uow.RollbackTransactionAsync();
                //_logger.LogError(ex, "Error al crear factura");
                throw new Exception("No se pudo procesar la factura. Intente de nuevo.");
            }
        }

        public async Task<Result<InvoiceDto>> UpdateAsync(int id, InvoiceDto dto)
        {
            var invoice = await _uow.Invoices.FindAsync(id);
            if (invoice == null)
                return Result<InvoiceDto>.Failure($"No existe la factura con el Id {id}.");

            if (invoice.StatusId == (int)InvoiceStatusEnum.Pagada || invoice.StatusId == (int)InvoiceStatusEnum.Anulada || invoice.StatusId == (int)InvoiceStatusEnum.Reembolsada)
                return Result<InvoiceDto>.Failure("La factura ya no se puede modificar.");

            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<InvoiceDto>.Failure("El paciente no existe o no pertenece a esta clínica.");
            
            _mapper.Map(dto,  invoice);
            invoice.Id = id;

            await _uow.Invoices.UpdateAsync(invoice);
            await _uow.CompleteAsync();

            return Result<InvoiceDto>.Success(_mapper.Map<InvoiceDto>(invoice));
        }

        private async Task<string> GenerateInvoiceNumberAsync()
        {

            var invoices = await _uow.Invoices.GetAllAsync();
            var lastInvoice = invoices
                .OrderByDescending(i => i.Id)
                .Select(i => i.Number)
                .First();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastInvoice))
            {
                var parts = lastInvoice.Split('-');
                if (parts.Length > 0 && int.TryParse(parts.Last(), out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            int currentYear = DateTime.Now.Year;
            return $"FAC-{currentYear}-{nextNumber.ToString().PadLeft(6, '0')}";// Ejemplo: FAC-000001
        }
    }
}
