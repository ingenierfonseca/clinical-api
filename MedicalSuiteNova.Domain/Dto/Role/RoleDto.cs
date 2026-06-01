namespace MedicalSuiteNova.Domain.Dto.Role
{
    public class RoleDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
