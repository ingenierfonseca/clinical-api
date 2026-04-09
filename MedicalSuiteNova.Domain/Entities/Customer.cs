using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        public byte Age { get; set; }
        [StringLength(15)]
        public string? Phone { get; set; }
        [StringLength(60)]
        public string? Email { get; set; }
        [StringLength(500)]
        public string? Avatar { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        public string getShortName()
        {
            return $"{FirstName.Split(' ', 2)[0].Trim()} {LastName.Split(' ', 2)[0].Trim()}";
        }
    }
}
