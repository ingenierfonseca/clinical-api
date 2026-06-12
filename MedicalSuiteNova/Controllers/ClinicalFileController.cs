using MedicalSuiteNova.Application.Interfaces;
using MedicalSuiteNova.Domain.Dto.ClinicalFile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSuiteNova.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clinical-file")]
    public class ClinicalFileController(IClinicalFileService clinicalFileService) : Controller
    {
        private readonly IClinicalFileService _clinicalFileService = clinicalFileService;

        [HttpPost("upload-file")]
        public async Task<IActionResult> UploadFile([FromForm] CreateClinicalFileDto item, [FromForm] IFormFile file)
        {
            var result = await _clinicalFileService.UploadFile(item, file);
            return result.IsSuccess ? Ok(result) : BadRequest(new { message = result.ErrorMessage });
        }

        [HttpGet("session/{id:int}/images")]
        public async Task<IActionResult> GetSessionImages(int id)
        {
            var items = await _clinicalFileService.GetSessionImages(id);
            return Ok(items);
        }
    }
}
