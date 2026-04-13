using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Domain.Interfaces
{
    public interface IAppointmentTypeRepository: IBaseRepository<AppointmentType>
    {
        public Task<bool> IsValidAsync(int id);
    }
}
