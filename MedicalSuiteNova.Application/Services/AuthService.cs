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

            return await BuildResponse(user, token, refreshToken);
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

            return await BuildResponse(user, token, refreshToken);
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

            return await BuildResponse(user, token, newRefreshToken);
        }

        public async Task<UserInfoDto> GetCurrentUserAsync(string username)
        {
            var user = await _uow.Users.GetByUsernameAsync(username)
                ?? throw new KeyNotFoundException("Usuario no encontrado");

            var roles = user.UserRoles?.Select(ur => ur.Role?.Name ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList() ?? new();
            var permissions = user.UserRoles?
                .SelectMany(ur => ur.Role?.RolePermissions ?? new List<RolePermission>())
                .Select(rp => rp.Permission?.Name ?? "")
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList() ?? new();

            return new UserInfoDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Roles = roles,
                Permissions = permissions,
                IsActive = user.IsActive,
                DoctorId = user.DoctorId,
                DoctorName = user.Doctor != null ? $"{user.Doctor.FirstName} {user.Doctor.LastName}" : null,
                CustomerId = user.CustomerId,
                CustomerName = user.Customer != null ? $"{user.Customer.FirstName} {user.Customer.LastName}" : null,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<LinkUserResultDto> LinkDoctorAsync(int userId, int doctorId)
        {
            var user = await _uow.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("Usuario no encontrado");

            var doctor = await _uow.Doctors.FindAsync(doctorId)
                ?? throw new KeyNotFoundException("Doctor no encontrado");

            user.DoctorId = doctorId;
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return new LinkUserResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                DoctorId = doctorId,
                CustomerId = user.CustomerId,
                Message = $"Usuario vinculado al doctor {doctor.FirstName} {doctor.LastName} exitosamente"
            };
        }

        public async Task<LinkUserResultDto> LinkCustomerAsync(int userId, int customerId)
        {
            var user = await _uow.Users.FindAsync(userId)
                ?? throw new KeyNotFoundException("Usuario no encontrado");

            var customer = await _uow.Customers.FindAsync(customerId)
                ?? throw new KeyNotFoundException("Cliente no encontrado");

            user.CustomerId = customerId;
            await _uow.Users.UpdateAsync(user);
            await _uow.CompleteAsync();

            return new LinkUserResultDto
            {
                UserId = user.Id,
                Username = user.Username,
                DoctorId = user.DoctorId,
                CustomerId = customerId,
                Message = $"Usuario vinculado al cliente {customer.FirstName} {customer.LastName} exitosamente"
            };
        }

        private async Task<AuthResponseDto> BuildResponse(User user, string token, string refreshToken)
        {
            var roles = user.UserRoles?.Select(ur => ur.Role?.Name ?? "").Where(n => !string.IsNullOrEmpty(n)).ToList() ?? new();
            var permissions = user.UserRoles?
                .SelectMany(ur => ur.Role?.RolePermissions ?? new List<RolePermission>())
                .Select(rp => rp.Permission?.Name ?? "")
                .Where(n => !string.IsNullOrEmpty(n))
                .Distinct()
                .ToList() ?? new();

            return new AuthResponseDto
            {
                Status = 200,
                Token = token,
                RefreshToken = refreshToken,
                Username = user.Username,
                Roles = roles,
                Permissions = permissions,
                UserId = user.Id,
                DoctorId = user.DoctorId,
                CustomerId = user.CustomerId
            };
        }
    }
}
