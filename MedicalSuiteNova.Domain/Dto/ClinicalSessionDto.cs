
namespace MedicalSuiteNova.Domain.Dto
{
    public class ClinicalSessionDto
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date { get; set; }
        public string? ReasonForVisit { get; set; }
        public string? ClinicalNotes { get; set; }

        public virtual CustomerDto? Customer { get; set; }
        public virtual DoctorDto? Doctor { get; set; }
    }
}
