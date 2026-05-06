
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class ClinicalSessionService(IUnitOfWork uow, IMapper mapper) : BaseService<ClinicalSession>(uow, mapper, uow.ClinicalSessions), IClinicalSessionService
    {
        public async Task<Result<ClinicalSessionDto>> AddAsync(ClinicalSessionDto dto) 
        {
            if (!await _uow.Customers.ExistsAsync(dto.CustomerId))
                return Result<ClinicalSessionDto>.Failure("El CustomerId no es válido.");

            if (!await _uow.Doctors.ExistsAsync(dto.DoctorId))
                return Result<ClinicalSessionDto>.Failure("El DoctorId no es válido.");

            if (dto.Date == DateTime.MinValue)
                dto.Date = DateTime.Now;

            var session = _mapper.Map<ClinicalSession>(dto);
            await _uow.ClinicalSessions.AddAsync(session);
            await _uow.CompleteAsync();

            return Result<ClinicalSessionDto>.Success(_mapper.Map<ClinicalSessionDto>(session));
        }
    }
}
