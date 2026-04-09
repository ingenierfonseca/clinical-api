using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using MedicalSuiteNova.Infrastructure.Util;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }

        public async Task<bool> IsValidPatientAsync(int patientId)
        {
            return await _context.Patients.AnyAsync(p => p.Id == patientId);
        }

        public async Task<PagedResponse<Customer>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search)
        {
            var query = _context.Set<Customer>()
                .Where(a => search != string.Empty && a.FirstName.Contains(search) || a.LastName.Contains(search))
                .OrderBy(a => a.FirstName);
            return await GetAllAsync(pageNumber, pageSize, query);
        }

        public async Task<List<CustomerDashboardDto>> GetDashboard()
        {
            var dashboardList = new List<CustomerDashboardDto>();
            var now = DateTime.UtcNow;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddMonths(-1);

            var allCustomers = await _context.Set<Customer>().ToListAsync();
            int totalCount = allCustomers.Count;
            int totalLastMonth = allCustomers.Count(c => c.CreatedAt < firstDayCurrentMonth);
            int totalNewThisMonth = allCustomers.Count(c => c.CreatedAt >= firstDayCurrentMonth);
            var change = StatHelper.CalculateTrend(totalLastMonth, totalNewThisMonth);
            dashboardList.Add(new CustomerDashboardDto
            {
                Title = "Total Pacientes",
                Value = totalCount.ToString(),
                Change = change.ToString(),
                Trend = change >= 0 ? "Up" : "Down",
                Description = "vs last month"
            });

            return dashboardList;
        }
    }
}
