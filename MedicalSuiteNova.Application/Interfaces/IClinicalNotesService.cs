using MedicalSuiteNova.Domain.Dto.ClinicalNotes;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IClinicalNotesService : IBaseService<ClinicalNotes>
    {
        Task<Result<ClinicalNotesDto>> AddAsync(CreateClinicalNotesDto dto);
    }
}
