using MedicalSuiteNova.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService) => _tokenService = tokenService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            //if (login.Username == "admin" && login.Password == "123456")
            //{
                var token = _tokenService.CreateToken("admin");
                return Ok(new
                {
                    status = 200,
                    token,
                    username = "admin"
                });
            //}
            //throw new UnauthorizedAccessException("Usuario o contraseña incorrectos");
        }
    }
}
