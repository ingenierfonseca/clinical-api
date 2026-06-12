

using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Staff
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
        public bool IsActive { get; set; } = true;
        public string? Avatar { get; set; }
        public byte StaffTypeId { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual StaffType? StaffType { get; set; }

        public string GetShortName() => $"{FirstName.Split(' ', 2)[0]} {LastName.Split(' ', 2)[0]}";
    }
}
