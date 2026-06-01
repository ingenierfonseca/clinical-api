namespace MedicalSuiteNova.Domain.Dto.Role
{
    public class UpdateRoleDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
