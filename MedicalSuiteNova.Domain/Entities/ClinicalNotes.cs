
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ClinicalNotes
    {
        public int Id { get; set; }
        public long ClinicalSessionId { get; set; }
        public int DoctorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Note { get; set; } = string.Empty;
        public bool IsPrivate { get; set; }

        [ForeignKey("ClinicalSessionId")]
        public virtual ClinicalSession? Session { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }
    }
}
