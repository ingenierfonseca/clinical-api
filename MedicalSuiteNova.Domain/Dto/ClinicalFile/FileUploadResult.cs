
namespace MedicalSuiteNova.Domain.Dto.ClinicalFile
{
    public class FileUploadResult
    {
        public string OriginalName { get; set; } = string.Empty;
        public string StoredName { get; set; } = string.Empty;
        public string RelativePath { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long Size { get; set; }
    }
}
