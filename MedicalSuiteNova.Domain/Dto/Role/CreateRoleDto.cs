namespace MedicalSuiteNova.Domain.Dto.Role
{
    public class CreateRoleDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}
