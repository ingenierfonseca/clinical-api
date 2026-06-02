
namespace MedicalSuiteNova.Domain.Dto
{
    public class CreateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool IsActive { get; set; } = true;
        public int? StaffId { get; set; }
    }
}
