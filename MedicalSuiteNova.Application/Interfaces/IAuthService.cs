using MedicalSuiteNova.Domain.Dto.Auth;

namespace MedicalSuiteNova.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<UserInfoDto> GetCurrentUserAsync(string username);
        Task<LinkUserResultDto> LinkStaffAsync(int userId, int staffId);
    }
}
