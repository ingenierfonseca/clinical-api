using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto.Appointment
{
    public class AppointmentDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El CustomerId es obligatorio.")]
        public required int CustomerId { get; set; }

        [Required(ErrorMessage = "El DoctorId es obligatorio.")]
        public required int DoctorId { get; set; }

        public int? ResourceId { get; set; }

        [Required(ErrorMessage = "El AppointmentTypeId es obligatorio.")]
        public required byte AppointmentTypeId { get; set; }

        [Required(ErrorMessage = "La Fecha es obligatoria.")]
        [DataType(DataType.Date, ErrorMessage = "Formato de fecha no válido.")]
        public required DateOnly Date { get; set; }

        [Required(ErrorMessage = "La Hora de inicio es obligatoria.")]
        public required TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public byte StatusId { get; set; }

        public string? Notes { get; set; }

        public string? CancellationReason { get; set; }

        public bool IsConfirmed { get; set; }
    }
}
