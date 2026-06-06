
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Doctor: IEntity
    {
        public int Id { get; set; }
        public byte ServiceId { get; set; }
        public int StaffId { get; set; }
        public int SpecialtyId { get; set; }

        public object GetId() => Id;

        [ForeignKey("ServiceId")]
        public virtual Service? Service { get; set; }

        [ForeignKey("SpecialtyId")]
        public virtual Specialty? Specialty { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; }
    }
}
