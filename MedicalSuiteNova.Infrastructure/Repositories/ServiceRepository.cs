
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ServiceRepository(ApplicationDbContext context, IMapper mapper) : BaseRepository<Service>(context, mapper), IServiceRepository
    {
    }
}
