
using MedicalSuiteNova.Domain.Dto.Appointment;
using MedicalSuiteNova.Domain.Dto.Customer;
using MedicalSuiteNova.Domain.Dto.Doctor;

namespace MedicalSuiteNova.Domain.Dto
{
    public class ClinicalVisitsDto
    {
        public long Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime VisitDate { get; set; }
        public long? AppointmentId { get; set; }
        public int DoctorId { get; set; }
        public string? Notes { get; set; }

        public virtual CustomerDto? Customer { get; set; }
        public virtual DoctorDto? Doctor { get; set; }
        public virtual AppointmentDto? Appointment { get; set; }
    }
}
