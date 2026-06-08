using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalNotes;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ClinicalNotesService(IUnitOfWork uow, IMapper mapper) : BaseService<ClinicalNotes>(uow, mapper, uow.ClinicalNotes), IClinicalNotesService
    {
        public async Task<Result<ClinicalNotesDto>> AddAsync(CreateClinicalNotesDto dto)
        {
            if (!await _uow.ClinicalSessions.ExistsAsync(dto.ClinicalSessionId))
                return Result<ClinicalNotesDto>.Failure("El ClinicalSessionId no es válido.");

            if (!await _uow.Doctors.ExistsAsync(dto.DoctorId))
                return Result<ClinicalNotesDto>.Failure("El DoctorId no es válido.");

            var entity = _mapper.Map<ClinicalNotes>(dto);
            await _uow.ClinicalNotes.AddAsync(entity);
            await _uow.CompleteAsync();

            return Result<ClinicalNotesDto>.Success(_mapper.Map<ClinicalNotesDto>(entity));
        }

        public async Task<List<ClinicalNotes>> GetBySessionId(int sessionId)
        {
            return await _uow.ClinicalNotes.GetAllAsync(
                x => x.ClinicalSessionId == sessionId,
                query => query.OrderByDescending(a => a.CreatedAt),
                x => x.Doctor!,
                x => x.Doctor!.Staff!
            );
        }
    }
}
