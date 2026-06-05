namespace MedicalSuiteNova.Domain.Dto
{
    public class ResourceDto
    {
        public int Id { get; set; }
        public byte ResourceTypeId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int Capacity { get; set; }
        public string? Color { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
