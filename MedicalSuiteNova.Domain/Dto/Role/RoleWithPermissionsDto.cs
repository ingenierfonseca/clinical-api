namespace MedicalSuiteNova.Domain.Dto.Role
{
    public class RoleWithPermissionsDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int> PermissionIds { get; set; } = new();
    }
}
