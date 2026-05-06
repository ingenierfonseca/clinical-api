
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ClinicalSession: IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DoctorId { get; set; }
        public DateTime Date {  get; set; }
        public string? ReasonForVisit { get; set; }
        public string? ClinicalNotes { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
        public object GetId() => Id;
    }
}
