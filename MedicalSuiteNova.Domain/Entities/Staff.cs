
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Staff : IEntity
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public DateTime? HireDate { get; set; }
        public bool IsActive { get; set; } = true;
        public string? Avatar { get; set; }
        public byte StaffTypeId { get; set; }
        public DateTime? BirthDate { get; set; }

        public virtual StaffType? StaffType { get; set; }

        public object GetId() => Id;
    }
}
