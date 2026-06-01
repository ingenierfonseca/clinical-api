namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class UserInfoDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public bool IsActive { get; set; }
        public int? DoctorId { get; set; }
        public string? DoctorName { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
