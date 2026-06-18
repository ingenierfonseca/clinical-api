
using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto
{
    public class StaffDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; }
        public string? Avatar { get; set; }
        public byte StaffTypeId { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? UserId { get; set; }
    }
}
