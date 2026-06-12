using MedicalSuiteNova.Domain.Dto.ClinicalFile;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IClinicalFileService : IBaseService<ClinicalFile>
    {
        Task<Result<ClinicalFileDto>> UploadFile(CreateClinicalFileDto item, IFormFile file);
        Task<List<ClinicalFile>> GetSessionImages(int sessionId);
    }
}
