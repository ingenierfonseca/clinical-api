
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ClinicalVisits: IEntity
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime VisitDate { get; set; }
        public long? AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string? Notes { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual Customer? Customer { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor? Doctor { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public virtual Appointment? Appointment { get; set; }

        public object GetId() => Id;
    }
}
