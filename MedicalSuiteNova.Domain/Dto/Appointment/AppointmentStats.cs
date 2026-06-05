
namespace MedicalSuiteNova.Domain.Dto.Appointment
{
    public class AppointmentStats
    {
        public int Total { get; set; }
        public int Confirmed { get; set; }
        public int Pending { get; set; }
        public int Cancelled { get; set; }
    }
}
