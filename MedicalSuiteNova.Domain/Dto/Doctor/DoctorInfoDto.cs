namespace MedicalSuiteNova.Domain.Dto.Doctor
{
    public class DoctorInfoDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateOnly? BirthDate { get; set; }
        public string? Avatar { get; set; }
        public string? Specialty { get; set; }
        public string? Service { get; set; }
    }
}
