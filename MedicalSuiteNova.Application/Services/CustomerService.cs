using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Util;

namespace MedicalSuiteNova.Application.Services
{
    public class CustomerService : BaseService<Customer>, ICustomerService
    {
        public CustomerService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Customers) { }

        public async Task<List<CustomerDashboardDto>> GetDashboard()
        {
            var dashboardList = new List<CustomerDashboardDto>();
            var now = DateTime.UtcNow;
            var firstDayCurrentMonth = new DateTime(now.Year, now.Month, 1);
            var firstDayLastMonth = firstDayCurrentMonth.AddMonths(-1);

            var allCustomers = await _uow.Customers.GetAllAsync();
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

        public async Task<PagedResponse<Customer>> GetAllPaginatedAsync(int pageNumber, int pageSize, string search)
        {
            return await _uow.Customers.GetAllPaginatedAsync(pageNumber, pageSize, search);
        }
    }
}
