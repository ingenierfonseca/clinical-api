using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalSuiteNova.Domain.Entities
{
    public class Appointment
    {
        public long Id { get; set; }
        public required int CustomerId { get; set; }
        public required int DoctorId { get; set; }
        public int? ResourceId { get; set; }
        public required byte AppointmentTypeId { get; set; }
        public required DateOnly Date { get; set; }
        public required TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public byte StatusId { get; set; }
        public string? Notes { get; set; }
        public string? CancellationReason { get; set; }
        public bool IsConfirmed { get; set; }
        public bool ReminderSent { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }


        [ForeignKey("CustomerId")]
        public virtual Customer? Patient { get; set; }

        [ForeignKey("DoctorId")]
        public virtual Doctor? Doctor { get; set; }

        [ForeignKey("ResourceId")]
        public virtual Resource? Resource { get; set; }

        [ForeignKey("AppointmentTypeId")]
        public virtual AppointmentType? AppointmentType { get; set; }

        [ForeignKey("StatusId")]
        public virtual AppointmentStatus? Status { get; set; }
    }
}
