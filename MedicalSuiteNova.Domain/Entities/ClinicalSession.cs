
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ClinicalSession
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date {  get; set; }
        public string? ReasonForVisit { get; set; }
        public byte ConsultationSpecialtyId { get; set; }
        public byte ConsultationTypeId { get; set; }
        public long? ConsultationId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }

        [ForeignKey("ConsultationSpecialtyId")]
        public virtual Service? ConsultationSpecialty { get; set; }

        [ForeignKey("ConsultationTypeId")]
        public virtual ConsultationType? ConsultationType { get; set; }
    }
}
