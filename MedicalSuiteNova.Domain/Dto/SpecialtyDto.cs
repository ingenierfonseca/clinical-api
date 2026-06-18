
namespace MedicalSuiteNova.Domain.Dto
{
    public class SpecialtyDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public byte ServiceId { get; set; }
    }
}
