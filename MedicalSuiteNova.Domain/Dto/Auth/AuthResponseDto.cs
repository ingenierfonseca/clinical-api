namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class AuthResponseDto
    {
        public int Status { get; set; }
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public required string Username { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public int UserId { get; set; }
        public int? DoctorId { get; set; }
        public int? CustomerId { get; set; }
    }
}
