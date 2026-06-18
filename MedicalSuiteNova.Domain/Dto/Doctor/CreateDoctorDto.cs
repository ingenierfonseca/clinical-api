
using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto.Doctor
{
    public class CreateDoctorDto
    {
        public byte ServiceId { get; set; }
        public int StaffId { get; set; }
        public int SpecialtyId { get; set; }
        [StringLength(10)]
        public string Title { get; set; } = string.Empty;
    }
}
