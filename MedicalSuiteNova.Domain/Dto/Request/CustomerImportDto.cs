

namespace MedicalSuiteNova.Domain.Dto.Request
{
    public class CustomerImportDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte Age { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
