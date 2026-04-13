
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IDoctorRepository: IBaseRepository<Doctor>
    {
        public Task<bool> IsValidAsync(int id);
    }
}
