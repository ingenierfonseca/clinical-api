using MedicalSuiteNova.Api.Constants;
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MedicalSuiteNova.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var username = User.FindFirstValue("sub")
                ?? User.FindFirstValue(JwtRegisteredClaimNames.UniqueName)
                ?? User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(username))
                return Unauthorized(new { status = 401, errors = new[] { AppError.InvalidToken } });

            var result = await _authService.GetCurrentUserAsync(username);
            return Ok(result);
        }

        [HttpPost("{userId}/link-staff/{doctorId}")]
        [Authorize(Roles = AppRole.SuperAdmin)]
        public async Task<IActionResult> LinkStaffAsync(int userId, int doctorId)
        {
            var result = await _authService.LinkStaffAsync(userId, doctorId);
            return Ok(result);
        }
    }
}
