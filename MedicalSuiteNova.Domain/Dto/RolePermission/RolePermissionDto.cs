
namespace MedicalSuiteNova.Domain.Dto.RolePermission
{
    public class RolePermissionDto
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public string? RoleName { get; set; }
        public string? PermissionName { get; set; }
        public string? PermissionModule { get; set; }
    }
}
