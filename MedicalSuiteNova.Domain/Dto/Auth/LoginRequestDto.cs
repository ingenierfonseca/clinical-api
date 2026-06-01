namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class LoginRequestDto
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
