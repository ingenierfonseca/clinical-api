using AutoMapper;
using AutoMapper.QueryableExtensions;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<InvoiceItemInfoDto?> GetByIdDtoAsync(int id)
        {
            return await _dbSet
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
                .Include(a => a.Currency)
                .Select(c => new CustomerInvoiceDashboardDto
                {
                    Id = c.Id,
                    Avatar = c.Avatar,
                    FullName = c.FirstName.Trim() + " " + c.LastName.Trim(),
                    Age = c.Age,
                    Balance = c.Balance,
                    Currency = c.Currency!.Symbol ?? "",
                    LastPayment = c.Invoices.SelectMany(i => i.Payments).Any()
                    ? c.Invoices.SelectMany(i => i.Payments).Max(p => p.Date)
                    : null,
                    LastVisit = c.ClinicalVisits.Max(p => p.VisitDate),
                    CountPaid = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == (int)InvoiceStatusEnum.Paid) : 0,
                    CountPending = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId == (int)InvoiceStatusEnum.Pending || i.StatusId == (int)InvoiceStatusEnum.PartialPayment) : 0,
                    CountOverdue = c.Invoices.Any() ? c.Invoices.Count(i => i.StatusId != (int)InvoiceStatusEnum.Overdue && (i.StatusId != (int)InvoiceStatusEnum.Paid && i.DueDate < hoy)) : 0
                });

            return await GetAllAsync(pageNumber, pageSize, query);
        }

        public async Task<List<InvoiceInfoDto>> GetInvoicesByCustomerAsync(int id)
        {
            return await _dbSet
                .Where(a => a.CustomerId == id)
                .OrderByDescending(a => a.IssueDate)
                .ProjectTo<InvoiceInfoDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<PaymentDetailDTO>> GetPaymentsByCustomer(int id)
        {
            return await _dbSet
                .Where(a => a.CustomerId == id)
                .SelectMany(i => i.Payments, (invoice, payment) => new PaymentDetailDTO
                {
                    Id = payment.Id,
                    InvoiceNumber = invoice.Number!,
                    Amount = payment.Amount,
                    PaymentTypeName = payment.PaymentType!.Name,
                    Date = payment.Date
                })
                .ToListAsync();
        }

        public async Task<string> GetLastInvoiceNumberAsync()
        {
            return await _dbSet
                .OrderByDescending(i => i.Id)
                .Select(i => i.Number)
                .FirstOrDefaultAsync()?? string.Empty;
        }
    }
}
