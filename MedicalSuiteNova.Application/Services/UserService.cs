
using AutoMapper;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class UserService(IUnitOfWork uow, IMapper mapper) : BaseService<User>(uow, mapper, uow.Users), IUserService
    {
        public async Task<UserDto> AddAsync(CreateUserDto dto)
        {
            var entity = _mapper.Map<User>(dto);
            entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            await _repository.AddAsync(entity);
            await _uow.CompleteAsync();
            return _mapper.Map<UserDto>(entity);
        }
    }
}
