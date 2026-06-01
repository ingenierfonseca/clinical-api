using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public int? DoctorId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Doctor? Doctor { get; set; }
        public virtual Customer? Customer { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public object GetId() => Id;
    }
}
