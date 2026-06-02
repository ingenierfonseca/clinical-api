
namespace MedicalSuiteNova.Domain.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public int? StaffId { get; set; }
        public string? StaffName { get; set; }
        public string? StaffTypeName { get; set; }
        public List<string> Roles { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
