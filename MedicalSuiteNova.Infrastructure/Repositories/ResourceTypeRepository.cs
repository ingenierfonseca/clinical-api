using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class ResourceTypeRepository : BaseRepository<ResourceType>, IResourceTypeRepository
    {
        public ResourceTypeRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
