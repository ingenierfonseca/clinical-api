
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentTypeRepository: BaseRepository<AppointmentType>, IAppointmentTypeRepository
    {
        public AppointmentTypeRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) {}
    }
}
