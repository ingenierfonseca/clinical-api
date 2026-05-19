
namespace MedicalSuiteNova.Domain.Dto
{
    public class AppointmentInfoDto
    {
        public long Id { get; set; }
        public required string PatientName {  get; set; }
        public DateTime AppointmentDate { get; set; }
        public required string DoctorName    { get; set; }
        public required string TypeName { get; set; }
    }
}
