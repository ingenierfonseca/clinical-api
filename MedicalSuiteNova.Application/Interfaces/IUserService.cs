
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        Task<UserDto> AddAsync(CreateUserDto dto);
    }
}
