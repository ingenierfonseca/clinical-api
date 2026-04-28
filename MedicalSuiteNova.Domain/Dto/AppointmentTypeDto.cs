
namespace MedicalSuiteNova.Domain.Dto
{
    public class AppointmentTypeDto
    {
        public byte Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public TimeOnly Time { get; set; }
    }
}
