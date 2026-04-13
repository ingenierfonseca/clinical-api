using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Interfaces;

namespace MedicalSuiteNova.Application.Services
{
    public class BaseService<T> : IBaseService<T> 
        where T : class, IEntity
    {
        public readonly IUnitOfWork _uow;

        protected readonly IBaseRepository<T> _repository;
        protected readonly IMapper _mapper;
        public BaseService(IUnitOfWork uow, IMapper mapper, IBaseRepository<T> repository)
        {
            _uow = uow;
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<PagedResponse<T>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<T?> FyndAsync(int id)
        {
            return await _repository.FyndAsync(id);
        }

        public async Task<T> AddAsync(T t)
        {
            var result = await _repository.AddAsync(t);
            await _uow.CompleteAsync();
            return result;
        }

        public async Task<T> UpdateAsync(T t)
        {
            var idValue = t.GetId();
            if (idValue == null || idValue.ToString() == "0")
                throw new ArgumentException("No se puede actualizar una entidad con Id 0 o null.");

            var entity = await _repository.ExistsAsync(idValue);
            if (!entity)
                throw new KeyNotFoundException($"No se encontró la entidad con ID {idValue}.");

            var result = await _repository.UpdateAsync(t);
            await _uow.CompleteAsync();
            return result;
        }
    }
}
