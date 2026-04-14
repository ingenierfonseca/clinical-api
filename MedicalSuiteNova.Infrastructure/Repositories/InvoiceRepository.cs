using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using MedicalSuiteNova.Application.Enums;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<bool> IsValidInvoiceAsync(int invoiceId)
        {
            return await _context.Invoices.AnyAsync(p => p.Id == invoiceId);
        }

        public IQueryable<InvoiceItemInfoDto> GetInvoicesAsQueryable()
        {
            return _context.Set<Invoice>()
                .OrderByDescending(a => a.CreatedAt)
                .ProjectTo<InvoiceItemInfoDto>(_mapper.ConfigurationProvider).AsQueryable();
        }

        public async Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id)
        {
            return await _context.Invoices
                .Where(a => a.Id == id)
                .ProjectTo<InvoiceItemInfoDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
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

                    CountPaid = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == (int)InvoiceStatusEnum.Pagada) : 0,
                    CountPending = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == (int)InvoiceStatusEnum.Pendiente || i.StatusId == (int)InvoiceStatusEnum.PagoParcial) : 0,
                    CountOverdue = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId != (int)InvoiceStatusEnum.Vencida && (i.StatusId != (int)InvoiceStatusEnum.Pagada && i.DueDate < hoy)) : 0
                });

            return await GetAllAsync(pageNumber, pageSize, query);
        }

        public async Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id)
        {
            return await _context.Invoices
                .Where(a => a.CustomerId == id)
                .OrderByDescending(a => a.IssueDate)
                .ProjectTo<InvoiceInfoDto>(_mapper.ConfigurationProvider)
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
    }
}
