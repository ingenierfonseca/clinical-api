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

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(int pageNumber, int pageSize) where TDto : class
            => await GetAllAsync<TDto>(pageNumber, pageSize, null, null);

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(int pageNumber, int pageSize, Expression<Func<T, bool>> predicate) where TDto : class
            => await GetAllAsync<TDto>(pageNumber, pageSize, predicate, null);

        public async Task<PagedResponse<TDto>> GetAllAsync<TDto>(
            int pageNumber, 
            int pageSize,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            params Expression<Func<T, object>>[] includes
        ) where TDto : class
        {
            var result = await _repository.GetAllAsync<TDto>(
                pageNumber: pageNumber,
                pageSize: pageSize,
                predicate: predicate,
                orderBy: orderBy, 
                includes: includes
            );
            return result;
        }

        public async Task<T?> FindAsync(int id)
        {
            return await _repository.FindAsync(id);
        }

        public async Task<Dto?> FirstOrDefaultAsync<Dto>(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include)
        {
            var entity = await _repository.FirstOrDefaultAsync(predicate, include);
            return _mapper.Map<Dto>(entity);
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

            await _repository.UpdateAsync(t);
            await _uow.CompleteAsync();

            return Result<Dto>.Success(_mapper.Map<Dto>(t));
        }
    }
}
