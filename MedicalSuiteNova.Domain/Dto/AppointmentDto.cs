
using System.ComponentModel.DataAnnotations;

namespace MedicalSuiteNova.Domain.Dto
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El CustomerId es obligatorio.")]
        public required int CustomerId { get; set; }

        [Required(ErrorMessage = "AppointmentDate es requerido.")]
        [DataType(DataType.DateTime, ErrorMessage = "Formato de fecha no válido.")]
        public DateTime AppointmentDate { get; set; }

        [Required(ErrorMessage = "El DoctorId es obligatorio.")]
        public required int DoctorId { get; set; }

        [Required(ErrorMessage = "El AppointmentTypeId es obligatorio.")]
        public required byte AppointmentTypeId { get; set; }
    }
}
