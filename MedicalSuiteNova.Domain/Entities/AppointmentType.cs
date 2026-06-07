
using MedicalSuiteNova.Domain.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class AppointmentType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public int DurationMinutes {  get; set; }
    }
}
