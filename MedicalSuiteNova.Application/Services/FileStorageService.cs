
using MedicalSuiteNova.Application.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalFile;
using MedicalSuiteNova.Domain.Dto.Responses;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Services
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<Result<FileUploadResult>> SaveAsync(
            IFormFile file,
            string folder,
            string[] allowedExtensions)
        {
            if (file == null || file.Length == 0)
                return Result<FileUploadResult>.Failure("Archivo inválido.");

            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return Result<FileUploadResult>.Failure(
                    $"Extensión no permitida: {extension}");

            string uploadsPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                FolderName.RootFolder,
                FolderName.UploadFolder,
                folder);

            Directory.CreateDirectory(uploadsPath);

            string storedName = $"{Guid.NewGuid():N}{extension}";

            string fullPath = Path.Combine(
                uploadsPath,
                storedName);

            await using var stream = new FileStream(
                fullPath,
                FileMode.Create);

            await file.CopyToAsync(stream);

            return Result<FileUploadResult>.Success(
                new FileUploadResult
                {
                    OriginalName = file.FileName,
                    StoredName = storedName,
                    RelativePath = $"{FolderName.UploadFolder}/{folder}/{storedName}",
                    ContentType = file.ContentType,
                    Size = file.Length
                });
        }
    }
}
