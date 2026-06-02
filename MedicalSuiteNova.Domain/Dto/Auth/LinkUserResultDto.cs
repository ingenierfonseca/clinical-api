namespace MedicalSuiteNova.Domain.Dto.Auth
{
    public class LinkUserResultDto
    {
        public int UserId { get; set; }
        public required string Username { get; set; }
        public int? StaffId { get; set; }
        public required string Message { get; set; }
    }
}
