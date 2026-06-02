
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class StaffTypeRepository : BaseRepository<StaffType>, IStaffTypeRepository
    {
        public StaffTypeRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) { }
    }
}
