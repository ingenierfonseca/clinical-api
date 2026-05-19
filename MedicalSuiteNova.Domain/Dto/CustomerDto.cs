
using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        [StringLength(20)]
        public required string DNI { get; set; }
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
        public decimal Balance { get; set; }
        public byte CurrencyId { get; set; }

        public virtual CurrencyDto? Currency { get; set; }

        public string GetShortName()
        {
            return $"{FirstName.Split(' ', 2)[0].Trim()} {LastName.Split(' ', 2)[0].Trim()}";
        }
    }
}
