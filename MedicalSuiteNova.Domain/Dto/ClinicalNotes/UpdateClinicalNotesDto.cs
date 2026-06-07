namespace MedicalSuiteNova.Domain.Dto.ClinicalNotes
{
    public class UpdateClinicalNotesDto
    {
        public long ClinicalSessionId { get; set; }
        public int DoctorId { get; set; }
        public string Note { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }
    }
}
