using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class ClinicalFile
    {
        public int Id { get; set; }
        public long ClinicalSessionId { get; set; }
        public int CustomerId { get; set; }
        public byte TypeId { get; set; }
        public required string Url { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("ClinicalSessionId")]
        public virtual ClinicalSession? Session { get; set; }

    }
}
