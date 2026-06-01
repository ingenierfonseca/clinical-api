using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Permission : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string Module { get; set; } = "General";

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

        public object GetId() => Id;
    }
}
