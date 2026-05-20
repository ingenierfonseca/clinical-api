
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdateAppointmentTypeDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public TimeOnly Time { get; set; }
    }
}
