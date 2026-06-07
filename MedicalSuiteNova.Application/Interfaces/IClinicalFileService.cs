
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IClinicalFileService : IBaseService<ClinicalFile>
    {
        Task<Result<ClinicalFileDto>> UploadFile(ClinicalFile item, IFormFile file);
    }
}
