namespace MedicalSuiteNova.Domain.Dto.Doctor
{
    public class DoctorInfoDto
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
