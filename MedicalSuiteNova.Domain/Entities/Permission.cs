
namespace MedicalSuiteNova.Domain.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string Module { get; set; } = "General";

        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
