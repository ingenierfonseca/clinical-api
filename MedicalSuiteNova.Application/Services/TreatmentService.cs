
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentService(IUnitOfWork uow, IMapper mapper) : BaseService<Treatment>(uow, mapper, uow.Treatments), ITreatmentService
    {
        public async Task<Result<TreatmentDto>> CreateAsync(TreatmentDto dto)
        {
            var exist = await _uow.Treatments.AnyAsync(x => x.Name == dto.Name);
            if (exist)
            {
                return Result<TreatmentDto>.Failure("Ya existe un tratamiento con ese nombre");
            }

            var treatment = _mapper.Map<Treatment>(dto);
            await _uow.Treatments.AddAsync(treatment);
            await _uow.CompleteAsync();
            return Result<TreatmentDto>.Success(_mapper.Map<TreatmentDto>(treatment));
        }
    }
}
