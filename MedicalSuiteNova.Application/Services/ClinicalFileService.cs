
using AutoMapper;
using MedicalSuiteNova.Application.Constants;
using MedicalSuiteNova.Application.Enums;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalFile;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace MedicalSuiteNova.Application.Services
{
    public class ClinicalFileService(IFileStorageService _fileStorage, IUnitOfWork uow, IMapper mapper) : BaseService<ClinicalFile>(uow, mapper, uow.ClinicalFiles), IClinicalFileService
    {
        public async Task<Result<ClinicalFileDto>> UploadFile(CreateClinicalFileDto item, IFormFile file)
        {
            if (!await _uow.Customers.ExistsAsync(item.CustomerId))
                return Result<ClinicalFileDto>.Failure("Cliente no encontrado.");

            if (!await _uow.ClinicalSessions.ExistsAsync(item.ClinicalSessionId))
                return Result<ClinicalFileDto>.Failure("Cliente no encontrado.");

            var folder = GetFolderByType(item.TypeId);
            var formats = GetFormatsByType(item.TypeId);

            var result = await _fileStorage.SaveAsync(
                file,
                $"{item.CustomerId}/{folder}",
                formats
            );

            if (!result.IsSuccess)
                return Result<ClinicalFileDto>.Failure(result.ErrorMessage);

            var entity = new ClinicalFile 
            {
                ClinicalSessionId = item.ClinicalSessionId,
                CustomerId = item.CustomerId,
                TypeId = item.TypeId,
                Description = item.Description,
                Url = result.Value.RelativePath
            };
            await _uow.ClinicalFiles.AddAsync(entity);
            await _uow.CompleteAsync();

            return Result<ClinicalFileDto>.Success(_mapper.Map<ClinicalFileDto>(entity));
        }

        public static string GetFolderByType(int typeId)
        {
            switch (typeId)
            {
                case (int)ImageTypeEnum.BeforeTreatment:
                case (int)ImageTypeEnum.Radiograph:
                case (int)ImageTypeEnum.EvolutionPhoto:
                    return FolderName.ClinicalImageFolder;
                default: throw new FileNotFoundException("");
            }
        }

        private static string[] GetFormatsByType(int typeId)
        {
            switch (typeId)
            {
                case (int)ImageTypeEnum.BeforeTreatment:
                case (int)ImageTypeEnum.Radiograph:
                case (int)ImageTypeEnum.EvolutionPhoto:
                    return [".jpg", ".jpeg", ".png", ".webp" ];
                default: throw new FileNotFoundException("");
            }
        }

        public async Task<List<ClinicalFile>> GetSessionImages(int sessionId)
        {
            return await _uow.ClinicalFiles.GetAllAsync(
                x => x.ClinicalSessionId == sessionId,
                query => query.OrderByDescending(x => x.CreatedAt),
                []
            );
        }
    }
}
