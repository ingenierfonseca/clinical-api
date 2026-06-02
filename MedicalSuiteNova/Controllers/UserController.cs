
using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto;
using MedicalSuiteNova.Domain.Dto.Responses;
using MedicalSuiteNova.Domain.Dto.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : Controller
    {
        private readonly IUserService _userService = userService;

        [HttpGet]
        public async Task<ActionResult<PagedResponse<UserDto>>> Get(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var items = await _userService.GetAllAsync<UserDto>(pageNumber, pageSize, null, null, x => x.UserRoles);
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _userService.FindAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUserDto p)
        {
            var result = await _userService.AddAsync(p);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, UpdateUserDto p)
        {
            var result = await _userService.UpdateAsync(id, p);
            return Ok(result);
        }
    }
}
