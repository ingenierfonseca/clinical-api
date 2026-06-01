namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class LinkUserResultDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public int? DoctorId { get; set; }
        public int? CustomerId { get; set; }
        public required string Message { get; set; }
    }
}
