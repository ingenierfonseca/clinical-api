using MedicalSuiteNova.Domain.Dto.Doctor;

namespace MedicalSuiteNova.Domain.Dto.ClinicalSession
{
    public class ClinicalSessionDto
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string? ReasonForVisit { get; set; }
        public byte ConsultationSpecialtyId { get; set; }
        public byte ConsultationTypeId { get; set; }
        public long ConsultationId { get; set; }

        public virtual CustomerDto? Customer { get; set; }
        public virtual DoctorDto? Doctor { get; set; }
        public virtual ServiceDto? ConsultationSpecialty { get; set; }
        public virtual ConsultationTypeDto? ConsultationType { get; set; }
    }
}
