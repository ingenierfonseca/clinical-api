
using AutoMapper;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Utils;

namespace MedicalSuiteNova.Application.Services
{
    public class InvoiceService(IUnitOfWork uow, IMapper mapper) : BaseService<Invoice>(uow, mapper, uow.Invoices), IInvoiceService
    {
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
            var validation = await ValidateCommonInvoiceLogic(dto);

            if (!validation.IsSuccess)
                return Result<ResponseInvoiceDto>.Failure(validation.ErrorMessage);

            DateTime dueDate = validation.Value.DueDate;

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
                    CreatedBy = "",//dto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    Items = []
                };

                var detailsResult = await ProcessInvoiceDetailsAsync(dto.Items, dto.CurrencyId);

                if (!detailsResult.IsSuccess) return Result<ResponseInvoiceDto>.Failure(detailsResult.ErrorMessage);

                invoice.Items = detailsResult.Value.Items;
                invoice.SubTotal = detailsResult.Value.SubTotal;
                invoice.TaxTotal = detailsResult.Value.Tax;
                invoice.Total = invoice.SubTotal + invoice.TaxTotal;

                var result = await AddAsync(invoice);

                await _uow.CommitTransactionAsync();

                return Result<ResponseInvoiceDto>.Success(ResponseInvoiceDto.ToDto(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                //_logger.LogError(ex, "Error al crear factura");
                throw new Exception("No se pudo procesar la factura. Intente de nuevo.");
            }
        }

        public async Task<Result<ResponseInvoiceDto>> UpdateAsync(int id, RequestInvoiceDto dto)
        {
            var validation = await ValidateCommonInvoiceLogic(dto);

            if (!validation.IsSuccess)
                return Result<ResponseInvoiceDto>.Failure(validation.ErrorMessage);

            var invoice = await _uow.Invoices.FirstOrDefaultAsync(
                i => i.Id == id,
                i => i.Payments
            );

            if (invoice == null)
                return Result<ResponseInvoiceDto>.Failure($"No existe la factura con el Id {id}.");

            if (invoice.StatusId == (int)InvoiceStatusEnum.Pagada || invoice.StatusId == (int)InvoiceStatusEnum.Anulada || invoice.StatusId == (int)InvoiceStatusEnum.Reembolsada)
                return Result<ResponseInvoiceDto>.Failure("La factura ya no se puede modificar.");

            if (dto.StatusId == (int)InvoiceStatusEnum.Pagada && invoice.Payments.Count == 0)
            {
                return Result<ResponseInvoiceDto>.Failure("No se pude modificar la factura al estado pagado, ya que no presenta pagos");
            }

            if (dto.StatusId == (int)InvoiceStatusEnum.Pagada && (invoice.Total - invoice.Payments.Sum(p => p.Amount)) > 0)
            {
                return Result<ResponseInvoiceDto>.Failure("No se pude modificar la factura al estado pagado, ya que la factura tiene saldo pendiente");
            }

            DateTime dueDate = validation.Value.DueDate;

            try
            {
                await _uow.BeginTransactionAsync();
                invoice.CustomerId = dto.CustomerId;
                invoice.CurrencyId = dto.CurrencyId;
                invoice.StatusId = dto.StatusId;
                invoice.PaymentTermId = dto.PaymentTermId;
                invoice.IssueDate = dto.IssueDate;
                invoice.DueDate = dueDate;
                invoice.Items = [];

                var detailsResult = await ProcessInvoiceDetailsAsync(dto.Items, dto.CurrencyId);

                if (!detailsResult.IsSuccess) return Result<ResponseInvoiceDto>.Failure(detailsResult.ErrorMessage);

                invoice.Items = detailsResult.Value.Items;
                invoice.SubTotal = detailsResult.Value.SubTotal;
                invoice.TaxTotal = detailsResult.Value.Tax;
                invoice.Total = invoice.SubTotal + invoice.TaxTotal;

                await _uow.InvoicesDetail.DeleteByInvoiceIdAsync(id);
                var result = await _uow.Invoices.UpdateAsync(invoice);
                await _uow.CommitTransactionAsync();

                return Result<ResponseInvoiceDto>.Success(ResponseInvoiceDto.ToDto(result));
            }
            catch (Exception)
            {
                await _uow.RollbackTransactionAsync();
                //_logger.LogError(ex, "Error al crear factura");
                throw new Exception("No se pudo procesar la factura. Intente de nuevo.");
            }
        }

        private async Task<Result<(DateTime DueDate, string Message)>> ValidateCommonInvoiceLogic(RequestInvoiceDto dto)
        {
            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<(DateTime, string)>.Failure("El paciente no existe o no pertenece a esta clínica.");

            if (!await _uow.Currencies.ExistsAsync(dto.CurrencyId))
                return Result<(DateTime, string)>.Failure("No se encontró el CurrencyId.");

            var term = await _uow.PaymentTerms.FindAsync(dto.PaymentTermId);
            if (term == null)
                return Result<(DateTime, string)>.Failure("No se encontró el PaymentTermId.");

            var dateValidation = DateTimeHelper.ValidateIssueDate(dto.IssueDate);
            if (!dateValidation.isValid)
                return Result<(DateTime, string)>.Failure(dateValidation.message);

            DateTime dueDate = dto.IssueDate.AddDays(term.DaysToDue);
            return Result<(DateTime, string)>.Success((dueDate, string.Empty));
        }

        private async Task<Result<(List<InvoiceItem> Items, decimal SubTotal, decimal Tax)>> ProcessInvoiceDetailsAsync(
            IEnumerable<RequestInvoiceItemDto> itemsDto, byte invoiceCurrencyId)
        {
            var processedItems = new List<InvoiceItem>();
            decimal runningSubTotal = 0;
            decimal runningTax = 0;

            foreach (var itemDto in itemsDto)
            {
                var treatment = await _uow.Treatments.FindAsync(x => x.Name == itemDto.Description);
                if (treatment == null)
                    return Result<(List<InvoiceItem>, decimal, decimal)>.Failure(
                        $"No se puede procesar la Factura: el Tratamiento {itemDto.Description} no existe.");

                decimal effectivePrice = treatment.Price;

                // Lógica de conversión de moneda
                if (invoiceCurrencyId != treatment.CurrencyId)
                {
                    var exchangeRate = await _uow.ExchangeRates.GetLatestRate(
                        fromCurrencyId: treatment.CurrencyId,
                        toCurrencyId: invoiceCurrencyId);

                    if (exchangeRate == 0)
                        return Result<(List<InvoiceItem>, decimal, decimal)>.Failure(
                            $"No hay tipo de cambio para convertir de la moneda del tratamiento a la de la factura.");

                    effectivePrice = treatment.Price * exchangeRate;
                }

                var lineTotal = (effectivePrice * itemDto.Quantity);
                runningSubTotal += effectivePrice;
                runningTax += itemDto.Tax;

                processedItems.Add(new InvoiceItem
                {
                    Description = itemDto.Description,
                    Quantity = itemDto.Quantity,
                    UnitPrice = effectivePrice,
                    TaxAmount = itemDto.Tax,
                    LineTotal = lineTotal,
                    OriginalCurrencyId = treatment.CurrencyId,
                    OriginalPrice = treatment.Price
                });
            }

            return Result<(List<InvoiceItem>, decimal, decimal)>.Success((processedItems, runningSubTotal, runningTax));
        }

        public async Task<string> GenerateInvoiceNumberAsync()
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
