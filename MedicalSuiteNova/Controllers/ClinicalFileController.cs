using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ClinicalFileController(IClinicalFileService clinicalFileService) : Controller
    {
        private readonly IClinicalFileService _clinicalFileService = clinicalFileService;

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile(ClinicalFile item, IFormFile file)
        {
            var result = await _clinicalFileService.UploadFile(item, file);
            return result.IsSuccess ? Ok(result) : BadRequest(new { message = result.ErrorMessage });
        }
    }
}
