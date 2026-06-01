namespace MedicalSuiteNova.Domain.Dto.Permission
{
    public class UpdatePermissionDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string Module { get; set; } = "General";
    }
}
