using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Appointment : IEntity
    {
        public long Id { get; set; }
        public required int CustomerId { get; set; }
        public required DateTime AppointmentDate { get; set; }
        public required int DoctorId { get; set; }
        public required byte AppointmentTypeId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Patient { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual AppointmentType? AppointmentType { get; set; }

        public object GetId() => Id;
    }
}
