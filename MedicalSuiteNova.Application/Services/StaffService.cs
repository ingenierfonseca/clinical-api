
using AutoMapper;
using MedicalSuiteNova.Application.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Services
{
    public class StaffService(IFileStorageService _fileStorage, IUnitOfWork uow, IMapper mapper) : BaseService<Staff>(uow, mapper, uow.Staff), IStaffService
    {
        public async Task<Result<string>> UploadAvatarAsync(int id, IFormFile file)
        {
            var staff = await _uow.Staff.FindAsync(id);

            if (staff == null)
                return Result<string>.Failure("Empleado no encontrado.");

            var result = await _fileStorage.SaveAsync(
                file,
                FolderName.AvatarStaffFolder,
                [".jpg", ".jpeg", ".png", ".webp"]);

            if (!result.IsSuccess)
                return Result<string>.Failure(result.ErrorMessage);

            if (!string.IsNullOrEmpty(staff.Avatar))
            {
                string oldFile = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    FolderName.RootFolder,
                    FolderName.UploadFolder,
                    FolderName.AvatarStaffFolder,
                    staff.Avatar);

                if (File.Exists(oldFile))
                    File.Delete(oldFile);
            }

            staff.Avatar = result.Value.StoredName;

            await _uow.Staff.UpdateAsync(staff);
            await _uow.CompleteAsync();

            return Result<string>.Success("");
        }
    }
}
