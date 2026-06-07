
namespace MedicalSuiteNova.Domain.Dto
{
    public class ClinicalFileDto
    {
        public int Id { get; set; }
        public long ClinicalSessionId { get; set; }
        public int CustomerId { get; set; }
        public byte TypeId { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }
    }
}
