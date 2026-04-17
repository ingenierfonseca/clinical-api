using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Interfaces;
using System.Linq.Expressions;

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

        public async Task<PagedResponse<Dto>> GetAllAsync<Dto>(int pageNumber, int pageSize, params Expression<Func<T, object>>[] includes) where Dto : class
        {

            var pageData = await _repository.GetAllAsync(pageNumber, pageSize, includes);
            var dtos = _mapper.Map<List<Dto>>(pageData.Data);

            return new PagedResponse<Dto>(dtos, pageNumber, pageSize, pageData.TotalItems);
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task<T> AddAsync(T t)
        {
            var result = await _repository.AddAsync(t);
            await _uow.CompleteAsync();
            return result;
        }

        public async Task<Dto> AddAsync<Dto>(Dto dto) where Dto: class
        {
            var entity = await _repository.AddAsync(_mapper.Map<T>(dto));
            await _uow.CompleteAsync();
            return _mapper.Map<Dto>(entity);
        }

        public async Task<Result<Dto>> UpdateAsync<Dto>(int id, Dto dto) where Dto : class
        {
            var t = await _repository.FindAsync(id)
                ?? throw new KeyNotFoundException($"No se encontró la entidad con ID {id}.");

            _mapper.Map(dto, t);

            var result = await _repository.UpdateAsync(t);
            await _uow.CompleteAsync();

            return Result<Dto>.Success(_mapper.Map<Dto>(t));
        }
    }
}
