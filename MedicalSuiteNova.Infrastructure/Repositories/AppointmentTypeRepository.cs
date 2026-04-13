
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class AppointmentTypeRepository: BaseRepository<AppointmentType>, IAppointmentTypeRepository
    {
        public AppointmentTypeRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) {}

        public async Task<bool> IsValidAsync(int id)
        {
            return await _context.AppointmentTypes.AnyAsync(p => p.Id == id);
        }
    }
}
