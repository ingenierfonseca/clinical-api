using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Infrastructure.Persistence;
using AutoMapper;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class CustomerRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<Customer>(context, mapper), ICustomerRepository
    {
    }
}
