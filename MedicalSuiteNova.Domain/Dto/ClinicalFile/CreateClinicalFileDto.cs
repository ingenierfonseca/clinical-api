
namespace MedicalSuiteNova.Domain.Dto.ClinicalFile
{
    public class CreateClinicalFileDto
    {
        public long ClinicalSessionId { get; set; }
        public int CustomerId { get; set; }
        public byte TypeId { get; set; }
        public required string Description { get; set; }
    }
}
