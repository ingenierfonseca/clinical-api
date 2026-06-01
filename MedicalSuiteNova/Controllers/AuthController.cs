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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService) => _authService = authService;

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
                return Unauthorized(new { status = 401, errors = new[] { "Token inválido" } });

            var result = await _authService.GetCurrentUserAsync(username);
            return Ok(result);
        }

        [HttpPost("{userId}/link-doctor/{doctorId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LinkDoctor(int userId, int doctorId)
        {
            var result = await _authService.LinkDoctorAsync(userId, doctorId);
            return Ok(result);
        }

        [HttpPost("{userId}/link-customer/{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LinkCustomer(int userId, int customerId)
        {
            var result = await _authService.LinkCustomerAsync(userId, customerId);
            return Ok(result);
        }
    }
}
