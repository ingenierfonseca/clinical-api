
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalSession;
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

            if (!await _uow.Services.ExistsAsync(dto.ConsultationSpecialtyId))
                return Result<ClinicalSessionDto>.Failure("ConsultationSpecialtyId no es válido.");

            if (!await _uow.ConsultationTypes.ExistsAsync(dto.ConsultationTypeId))
                return Result<ClinicalSessionDto>.Failure("ConsultationTypeId no es válido.");

            if (dto.Date == DateTime.MinValue)
                dto.Date = DateTime.UtcNow;

            var session = _mapper.Map<ClinicalSession>(dto);
            await _uow.ClinicalSessions.AddAsync(session);
            await _uow.CompleteAsync();

            return Result<ClinicalSessionDto>.Success(_mapper.Map<ClinicalSessionDto>(session));
        }

        public async Task<IEnumerable<ClinicalSessionShortInfoDto>> GetShortInfoByCustomer(int customerId)
        {
            var clinicalSession = await _uow.ClinicalSessions.GetAllAsync(
                c => c.CustomerId == customerId
            );

            return await _uow.ClinicalSessions.GetFilteredSelectedAsync(
                filter: session => session.CustomerId == customerId,
                select: session => new ClinicalSessionShortInfoDto
                {
                    Id = session.Id,
                    ConsultationNumber = $"CONSULT-{session.Id:D6}"
                }
            );
        }

        public async Task<List<ClinicalSession>> GetHistoryCustomer(int customerId)
        {
            return await _uow.ClinicalSessions.GetAllAsync(
                c => c.CustomerId == customerId,
                query => query.OrderByDescending(x => x.Date),
                []
            );
        }
    }
}
