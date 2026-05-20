
using AutoMapper;
using MedicalSuiteNova.Application.Constants;
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

            await _uow.BeginTransactionAsync();
            try
            {
                var detailsResult = await ProcessInvoiceDetailsAsync(dto.Items, dto.CurrencyId);

                if (!detailsResult.IsSuccess) 
                    return Result<ResponseInvoiceDto>.Failure(detailsResult.ErrorMessage);

                var invoice = await CreateBalanceInvoiceAsync(dto.CustomerId, dto.CurrencyId,
                    dto.StatusId,  dto.PaymentTermId, detailsResult.Value.SubTotal,
                    detailsResult.Value.Tax, dto.IssueDate, dueDate, "FAC", detailsResult.Value.Items);

                var result = await AddAsync(invoice);

                await _uow.CompleteAsync();
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

            await _uow.BeginTransactionAsync();
            try
            {
                var detailsResult = await ProcessInvoiceDetailsAsync(dto.Items, dto.CurrencyId);

                if (!detailsResult.IsSuccess) return Result<ResponseInvoiceDto>.Failure(detailsResult.ErrorMessage);

                invoice.CurrencyId = dto.CurrencyId;
                invoice.StatusId = dto.StatusId;
                invoice.PaymentTermId = dto.PaymentTermId;
                invoice.IssueDate = dto.IssueDate;
                invoice.DueDate = dueDate;
                invoice.Items = detailsResult.Value.Items;
                invoice.SubTotal = detailsResult.Value.SubTotal;
                invoice.TaxTotal = detailsResult.Value.Tax;
                invoice.Total = invoice.SubTotal + invoice.TaxTotal;

                await _uow.InvoicesDetail.DeleteByInvoiceIdAsync(id);
                await _uow.Invoices.UpdateAsync(invoice);

                await _uow.CompleteAsync();
                await _uow.CommitTransactionAsync();

                return Result<ResponseInvoiceDto>.Success(ResponseInvoiceDto.ToDto(invoice));
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

            var (isValid, message) = DateTimeHelper.ValidateIssueDate(dto.IssueDate);
            if (!isValid)
                return Result<(DateTime, string)>.Failure(message);

            DateTime dueDate = dto.IssueDate.AddDays(term.DaysToDue);
            return Result<(DateTime, string)>.Success((dueDate, string.Empty));
        }

        private async Task<Result<(List<InvoiceItem> Items, decimal SubTotal, decimal Tax)>> ProcessInvoiceDetailsAsync(
            IEnumerable<RequestInvoiceItemDto> itemsDto, byte invoiceCurrencyId)
        {
            var items = itemsDto.ToList();

            var descriptions = items
                .Select(x => x.Description.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var treatments = await _uow.Treatments.GetAllAsync(
                x => descriptions.Contains(x.Name));

            var treatmentDictionary = treatments.ToDictionary(
                x => x.Name,
                StringComparer.OrdinalIgnoreCase);

            var missingTreatments = descriptions
                .Where(x => !treatmentDictionary.ContainsKey(x))
                .ToList();

            if (missingTreatments.Count > 0)
            {
                return Result<(List<InvoiceItem>, decimal, decimal)>
                    .Failure(
                        $"No se pueden procesar los siguientes tratamientos porque no existen: {string.Join(", ", missingTreatments)}.");
            }

            var processedItems = new List<InvoiceItem>(items.Count);
            decimal subTotal = 0m;
            decimal taxTotal = 0m;

            var exchangeRateCache = new Dictionary<(byte From, byte To), decimal>();

            foreach (var item in items)
            {
                var treatment = treatmentDictionary[item.Description];

                decimal effectivePrice = treatment.Price;

                // Lógica de conversión de moneda
                if (invoiceCurrencyId != treatment.CurrencyId)
                {
                    var rateKey = (treatment.CurrencyId, invoiceCurrencyId);

                    if (!exchangeRateCache.TryGetValue(rateKey, out var exchangeRate))
                    {
                        exchangeRate = await _uow.ExchangeRates.GetLatestRate(
                            fromCurrencyId: treatment.CurrencyId,
                            toCurrencyId: invoiceCurrencyId);

                        if (exchangeRate <= 0)
                        {
                            return Result<(List<InvoiceItem>, decimal, decimal)>
                                .Failure(
                                    $"No existe tipo de cambio configurado de moneda {treatment.CurrencyId} a {invoiceCurrencyId}.");
                        }

                        exchangeRateCache[rateKey] = exchangeRate;
                    }

                    effectivePrice *= exchangeRate;
                }

                var lineTotal = effectivePrice * item.Quantity;
                subTotal += lineTotal;
                taxTotal += item.Tax;

                processedItems.Add(new InvoiceItem
                {
                    Description = item.Description,
                    Quantity = item.Quantity,
                    UnitPrice = effectivePrice,
                    TaxAmount = item.Tax,
                    LineTotal = lineTotal,
                    OriginalCurrencyId = treatment.CurrencyId,
                    OriginalPrice = treatment.Price
                });
            }

            return Result<(List<InvoiceItem>, decimal, decimal)>.Success((processedItems, subTotal, taxTotal));
        }

        public async Task<Invoice> CreateBalanceInvoicePlanAsync(string planName, int customerId, byte currencyId, decimal amount)
        {
            var items = new List<InvoiceItem>
            {
                new ()
                {
                    Description = $"{planName}",
                    Quantity = 1,
                    UnitPrice = amount,
                    LineTotal = amount,
                    OriginalCurrencyId = currencyId,
                    OriginalPrice = amount
                }
            };
            return await CreateBalanceInvoiceAsync(
                customerId, 
                currencyId, 
                (int)InvoiceStatusEnum.Pendiente, 
                1,
                amount, 
                0, 
                DateTime.UtcNow, 
                DateTime.UtcNow, 
                "PLAN", 
                items
            );
        }

        private async Task<Invoice> CreateBalanceInvoiceAsync(
            int customerId, 
            byte currencyId, 
            byte statusId, 
            byte paymentTermId, 
            decimal subTotal, 
            decimal tax,
            DateTime issueDate, 
            DateTime dueDate, 
            string originType,
            IReadOnlyCollection<InvoiceItem> items)
        {
            var invoiceNumber = await GenerateInvoiceNumberAsync();

            return new Invoice
            {
                CustomerId = customerId,
                CurrencyId = currencyId,
                StatusId = statusId,
                PaymentTermId = paymentTermId,
                Number = invoiceNumber,
                IssueDate = issueDate,
                DueDate = dueDate,
                SubTotal = subTotal,
                TaxTotal = tax,
                Total = subTotal + tax,
                CreatedBy = AuditConstants.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                OriginType = originType,
                Items = [.. items]
            };
        }

        public async Task<string> GenerateInvoiceNumberAsync()
        {
            var lastInvoice = await _uow.Invoices.GetLastInvoiceNumberAsync();

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
            // Ejemplo: FAC-000001
            return $"FAC-{currentYear}-{nextNumber.ToString().PadLeft(6, '0')}";
        }
    }
}
