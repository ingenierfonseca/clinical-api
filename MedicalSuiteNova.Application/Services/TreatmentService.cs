
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class TreatmentService: BaseService<Treatment>, ITreatmentService
    {
        public TreatmentService(IUnitOfWork uow, IMapper mapper) : base(uow, mapper, uow.Treatments) { }

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
