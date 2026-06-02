namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public List<string> Roles { get; set; } = [];
        public List<string> Permissions { get; set; } = [];
        public bool IsActive { get; set; }
        public int? StaffId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? StaffTypeName { get; set; }
        public string? Avatar {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
