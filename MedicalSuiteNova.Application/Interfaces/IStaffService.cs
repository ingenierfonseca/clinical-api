
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IStaffService : IBaseService<Staff>
    {
        Task<Result<string>> UploadAvatarAsync(int id, IFormFile file);
    }
}
