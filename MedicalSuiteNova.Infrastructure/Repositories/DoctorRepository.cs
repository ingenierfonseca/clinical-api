
using AutoMapper;
using MedicalSuiteNova.Domain.Entities;
using MedicalSuiteNova.Domain.Interfaces;
using MedicalSuiteNova.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MedicalSuiteNova.Infrastructure.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(ApplicationDbContext context, IMapper mapper) : base(context, mapper) {}

        public async Task<bool> IsValidAsync(int id)
        {
            return await _context.Doctors.AnyAsync(p => p.Id == id);
        }
    }
}
