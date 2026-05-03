
namespace MedicalSuiteNova.Domain.Dto.Update
{
    public class UpdateDoctorDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public byte Age { get; set; }
        public required string Specialist { get; set; }
        public required string Phone { get; set; }
    }
}
