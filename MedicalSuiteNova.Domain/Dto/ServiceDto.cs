
namespace MedicalSuiteNova.Domain.Dto
{
    public class ServiceDto
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
