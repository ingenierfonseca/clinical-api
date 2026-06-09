
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto.Customer
{
    public class UpdateCustomerDto : ICustomerValidatable
    {
        public string FirstName { get; set; } = string.Empty;
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        [StringLength(20)]
        public string Gender { get; set; } = string.Empty;
        [StringLength(100)]
        public string Address { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        [StringLength(15)]
        public string? Phone { get; set; }
        [StringLength(60)]
        public string? Email { get; set; }
    }
}
