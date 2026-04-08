using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Request;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context) : base(context) { }

        public async Task<PagedResponse<InvoiceDto>> GetAllPaginatedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Set<Invoice>()
                .OrderByDescending(a => a.CreatedAt)
                .Include(a => a.Patient)
                //.Include(a => a.InvoiceItem)
                .Select(a => new InvoiceDto
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    CustomerName = a.Patient.getShortName(),
                    Number = a.Number,
                    IssueDate = a.IssueDate,
                    DueDate = a.DueDate,
                    SubTotal = a.SubTotal,
                    TaxTotal = a.TaxTotal,
                    DiscountTotal = a.DiscountTotal,
                    Total = a.Total,
                    CurrencyId = a.CurrencyId,
                    StatusId = a.StatusId,
                    PaymentTermId = a.PaymentTermId,
                    CreatedBy = a.CreatedBy,
                    Items = a.Items!.Select(i => new InvoiceItemDto
                    {
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Tax = i.TaxAmount,
                        Discount = i.Discount,
                        LineTotal = i.LineTotal,
                    }).ToList()
                }) ;
            return await GetAllAsync(pageNumber, pageSize, query);
        }

        new public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            return await _context.Invoices
                .Where(a => a.Id == id)
                .Include(a => a.Patient)
                .Select(a => new InvoiceDto
                {
                    Id = a.Id,
                    CustomerId = a.CustomerId,
                    CustomerName = a.Patient.getShortName(),
                    Number = a.Number,
                    IssueDate = a.IssueDate,
                    DueDate = a.DueDate,
                    SubTotal = a.SubTotal,
                    TaxTotal = a.TaxTotal,
                    DiscountTotal = a.DiscountTotal,
                    Total = a.Total,
                    CurrencyId = a.CurrencyId,
                    StatusId = a.StatusId,
                    PaymentTermId = a.PaymentTermId,
                    CreatedBy = a.CreatedBy,
                    Items = a.Items!.Select(i => new InvoiceItemDto
                    {
                        Id= i.Id,
                        Description = i.Description,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice,
                        Tax = i.TaxAmount,
                        Discount = i.Discount,
                        LineTotal = i.LineTotal,
                    }).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<CustomerDashboardDto>> GetDashboard()
        {
            var dashboardList = new List<CustomerDashboardDto>();
            var now = DateTime.UtcNow;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddMonths(-1);

            var allInvoices = await _context.Invoices.ToListAsync();
            int totalCountPeding = allInvoices.Where(a => a.StatusId == 1).Count();
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
            var hoy = DateTime.Today;

            var query = _context.Set<Customer>()
                .Where(a => search != string.Empty && a.FirstName.Contains(search) || a.LastName.Contains(search))
                .OrderByDescending(a => a.Invoices.Select(i => i.StatusId).FirstOrDefault())
                .Select(c => new CustomerInvoiceDashboardDto
                {
                    Id = c.Id,
                    Avatar = c.Avatar,
                    FullName = c.FirstName.Trim() + " " + c.LastName.Trim(),
                    Age = c.Age,

                    // El balance es la suma de (Total Factura - Suma de Pagos de esa factura)
                    /*Balance = c.Invoices.Any() ? c.Invoices.Sum(i => i.Total - (decimal)i.Payments.Sum(p => p.Amount)) : 0,*/
                    // Agrupamos las facturas por moneda para obtener balances separados
                    Balances = c.Invoices
                    .GroupBy(i => new { i.Currency.Symbol, i.Currency.Code })
                    .Select(g => new CurrencyBalanceDto
                    {
                        Symbol = g.Key.Symbol,
                        Code = g.Key.Code,
                        Amount = g.Sum(i => i.Total - i.Payments.Sum(p => p.Amount))
                    }).ToList(),

                    // Buscamos la fecha máxima de todos los pagos de todas sus facturas
                    LastPayment = c.Invoices.SelectMany(i => i.Payments).Any()
                    ? c.Invoices.SelectMany(i => i.Payments).Max(p => p.Date)
                    : null, // El cast a nullable evita errores si no hay pagos

                    // Buscamos la fecha máxima de todas las visitas
                    LastVisit = c.Appointments.Max(p => p.AppointmentDate),

                    CountPaid = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == 2) : 0,     //"Paid"
                    CountPending = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == 1) : 0,  //"Pending"
                    CountOverdue = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId != 3 && i.DueDate < hoy) : 0
                });

            return await GetAllAsync(pageNumber, pageSize, query);
        }

        public async Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id)
        {
            return await _context.Invoices
                .Where(a => a.CustomerId == id)
                .Select(a => new InvoiceInfoDto
                {
                    Id = a.Id,
                    Number = a.Number,
                    IssueDate = a.IssueDate,
                    DueDate = a.DueDate,
                    SubTotal = a.SubTotal,
                    TaxTotal = a.TaxTotal,
                    DiscountTotal = a.DiscountTotal,
                    Total = a.Total - a.Payments.Sum(p => p.Amount),
                    Currency = a.Currency.Symbol,
                    Status = a.InvoiceStatus.Name,
                    PaymentTerm = a.PaymentTerm.Name,
                    StatusId = a.StatusId,
                })
                .ToListAsync();
        }

        public async Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id)
        {
            return await _context.Invoices
                .Where(a => a.CustomerId == id)
                .SelectMany(i => i.Payments, (invoice, payment) => new PaymentDetailDTO
                {
                    Id = payment.Id,
                    InvoiceNumber = invoice.Number,
                    Amount = payment.Amount,
                    PaymentTypeName = payment.PaymentType.Name,
                    Date = payment.Date
                })
                .ToListAsync();
        }

        public async Task<Result<ResponseInvoiceDto>> CreateInvoiceAsync(RequestInvoiceDto dto)
        {
            if (!await new CustomerRepository(_context).IsValidPatientAsync(dto.CustomerId))
            {
                return Result<ResponseInvoiceDto>.Failure("El paciente no existe o no pertenece a esta clínica.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoice = new Invoice
                {
                    CustomerId = dto.CustomerId,
                    CurrencyId = dto.CurrencyId,
                    StatusId = dto.StatusId,
                    PaymentTermId = dto.PaymentTermId,
                    Number = await GenerateInvoiceNumberAsync(),
                    IssueDate = dto.IssueDate,
                    DueDate = DateTime.UtcNow,
                    CreatedBy = dto.CreatedBy,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<InvoiceItem>()
                };

                decimal runningSubTotal = 0;
                decimal runningTax = 0;

                foreach (var itemDto in dto.Items)
                {
                    var tax = itemDto.Tax / 100;
                    var lineTax = (itemDto.UnitPrice * itemDto.Quantity * tax);
                    var lineTotal = (itemDto.UnitPrice * itemDto.Quantity) + lineTax;

                    var detail = new InvoiceItem
                    {
                        ProductId = itemDto.ProductId,
                        Description = itemDto.Description,
                        Quantity = itemDto.Quantity,
                        UnitPrice = itemDto.UnitPrice,
                        TaxAmount = lineTax,
                        LineTotal = lineTotal
                    };

                    invoice.Items!.Add(detail);

                    runningSubTotal += itemDto.UnitPrice * itemDto.Quantity;
                    runningTax += lineTax;
                }

                invoice.SubTotal = runningSubTotal;
                invoice.TaxTotal = runningTax;
                invoice.Total = runningSubTotal + runningTax;

                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Result<ResponseInvoiceDto>.Success(ResponseInvoiceDto.ToDto(invoice));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //_logger.LogError(ex, "Error al crear factura");
                throw new Exception("No se pudo procesar la factura. Intente de nuevo.");
            }
        }

        private async Task<string> GenerateInvoiceNumberAsync()
        {

            var lastInvoice = await _context.Invoices
                .OrderByDescending(i => i.Id)
                .Select(i => i.Number)
                .FirstOrDefaultAsync();

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
