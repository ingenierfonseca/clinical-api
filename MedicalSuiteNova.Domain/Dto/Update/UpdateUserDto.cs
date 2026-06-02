
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdateUserDto
    {
        public required string Username { get; set; }
        public required string Email { get; set; }
        public bool IsActive { get; set; }
        public int? StaffId { get; set; }
    }
}
