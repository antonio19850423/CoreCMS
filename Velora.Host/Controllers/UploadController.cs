using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;

        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("image")]
        public async Task<IActionResult> UploadImage([FromForm] UploadImageRequestDto request)
        {
            var result = await _uploadService.UploadImageAsync(request.File,request.Name);
            return StatusCode(result.StatusCode,result);
            }
        [HttpDelete("image")]
        public async Task<IActionResult> DeleteImage([FromQuery] string url)
        {
            var result = await _uploadService.DeleteImageAsync(url);

            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("replace")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> ReplaceImage(
           [FromForm] ReplaceImageRequestDto request)
        {
            var result = await _uploadService.ReplaceImageAsync(
                request.File,
                request.Name,
                request.OldUrl
            );

            return StatusCode(result.StatusCode, result);
        }
    }
}
