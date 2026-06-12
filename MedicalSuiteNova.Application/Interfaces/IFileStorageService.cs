using MedicalSuiteNova.Domain.Dto.ClinicalFile;
using MedicalSuiteNova.Domain.Dto.Responses;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<Result<FileUploadResult>> SaveAsync(
            IFormFile file,
            string folder,
            string[] allowedExtensions
        );
    }
}
