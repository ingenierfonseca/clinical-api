
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdateAppointmentTypeDto
    {
        public required string Name { get; set; }
        public string Description { get; set; }
        public TimeOnly Time { get; set; }
    }
}
