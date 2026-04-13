using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }

        public async Task<bool> IsValidAsync(int patientId)
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
    }
}
