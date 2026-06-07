namespace MedicalSuiteNova.Domain.Dto.ClinicalNotes
{
    public class ClinicalNotesDto
    {
        public int Id { get; set; }
        public long ClinicalSessionId { get; set; }
        public int DoctorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
