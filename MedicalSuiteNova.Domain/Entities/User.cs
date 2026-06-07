
namespace MedicalSuiteNova.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
        public int? StaffId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual Staff? Staff { get; set; }
        public ICollection<UserRole> UserRoles { get; set; } = [];
        
        public object GetId() => Id;
    }
}
