
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class AppointmentStatus: IEntity
    {
        public byte Id { get; set; }
        public required string Name { get; set; }

        public object GetId() => Id;
    }
}
