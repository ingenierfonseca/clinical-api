using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Auth;
using MedicalSuiteNova.Domain.Entities;

namespace MedicalSuiteNova.Application.Services
{
    public class AuthService(IUnitOfWork uow, ITokenService tokenService) : IAuthService
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _uow.Users.GetByUsernameAsync(request.Username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("La cuenta está desactivada");

            var token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return BuildResponse(user, token, refreshToken);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _uow.Users.GetByUsernameAsync(request.Username);
            if (existingUser != null)
                throw new ArgumentException("El nombre de usuario ya existe");

            var defaultRole = await _uow.Roles.FirstOrDefaultAsync(r => r.Name == "Doctor")
                ?? throw new InvalidOperationException("No se encontró el rol por defecto 'Doctor'. Ejecute el seed de roles.");

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Users.AddAsync(user);
            await _uow.CompleteAsync();

            await _uow.Users.SetRolesAsync(user.Id, new List<int> { defaultRole.Id });

            var token = _tokenService.CreateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return BuildResponse(user, token, refreshToken);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await _uow.Users.GetByRefreshTokenAsync(request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry == null || user.RefreshTokenExpiry < DateTime.UtcNow)
                throw new UnauthorizedAccessException("Refresh token inválido o expirado");

            if (!user.IsActive)
                throw new UnauthorizedAccessException("La cuenta está desactivada");

            var token = _tokenService.CreateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return BuildResponse(user, token, newRefreshToken);
        }

        public async Task<UserInfoDto> GetCurrentUserAsync(string username)
        {
            var user = await _uow.Users.GetByUsernameAsync(username)
                ?? throw new KeyNotFoundException("Usuario no encontrado");

            var roles = user.UserRoles?.Select(ur => ur.Role?.Name ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList() ?? new();
            var permissions = user.UserRoles?
                .SelectMany(ur => ur.Role?.RolePermissions ?? [])
                .Select(rp => rp.Permission?.Name ?? "")
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList() ?? [];

            var staff = user.Staff;

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = roles,
                Permissions = permissions,
                IsActive = user.IsActive,
                StaffId = staff?.Id,
                FirstName = staff!.FirstName,
                LastName = staff!.LastName,
                Avatar = staff?.Avatar,
                StaffTypeName = staff?.StaffType?.Name,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<LinkUserResultDto> LinkStaffAsync(int userId, int staffId)
        {
            var user = await _uow.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("Usuario no encontrado");

            var staff = await _uow.Staff.FindAsync(staffId)
                ?? throw new KeyNotFoundException("Personal no encontrado");

            user.StaffId = staffId;
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return new LinkUserResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                StaffId = staffId,
                Message = $"Usuario vinculado al personal {staff.FirstName} {staff.LastName} exitosamente"
            };
        }

        private static AuthResponseDto BuildResponse(User user, string token, string refreshToken)
        {
            var roles = user.UserRoles?.Select(ur => ur.Role?.Name ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList() ?? new();
            var permissions = user.UserRoles?
                .SelectMany(ur => ur.Role?.RolePermissions ?? [])
                .Select(rp => rp.Permission?.Name ?? "")
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList() ?? [];

            var staff = user.Staff;

            return new AuthResponseDto
            {
                Status = 200,
                Token = token,
                RefreshToken = refreshToken,
                Username = user.Username,
                Roles = roles,
                Permissions = permissions,
                UserId = user.Id,
                StaffId = staff?.Id,
                /*FirstName = staff!.FirstName,
                LastName = staff!.LastName,
                StaffTypeName = staff?.StaffType?.Name,
                Avatar = staff?.Avatar*/
            };
        }
    }
}
