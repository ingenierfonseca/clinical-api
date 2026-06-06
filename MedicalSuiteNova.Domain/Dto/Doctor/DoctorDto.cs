using MedicalSuiteNova.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Dto.Doctor
{
    public class DoctorDto
    {
        public int Id { get; set; }
        public byte ServiceId { get; set; }
        public int StaffId { get; set; }
        public int SpecialtyId { get; set; }
        public virtual ServiceDto? Service { get; set; }
        public virtual SpecialtyDto? Specialty { get; set; }
        public virtual StaffDto? Staff { get; set; }
    }
}
