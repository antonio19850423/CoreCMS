using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
namespace Velora.Host.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController
        : ControllerBase
    {
        private readonly ICaptchaService
            _captchaService;

        public CaptchaController(
            ICaptchaService
                captchaService)
        {
            _captchaService =
                captchaService;
        }

        [HttpGet("Generate")]
        public async Task<IActionResult>
            Generate(
                CancellationToken
                    cancellationToken)
        {
            var result =
                await _captchaService
                    .GenerateAsync(
                        cancellationToken);

            return Ok(
                new ResultDto<
                    CaptchaGenerateDto>
                {
                    Success = true,

                    Message =
                        "کد امنیتی با موفقیت ایجاد شد.",

                    Data =
                        result
                });
        }

        [HttpPost("Validate")]
        public async Task<IActionResult>
            Validate(
                [FromBody]
            CaptchaValidateDto input,

                CancellationToken
                    cancellationToken)
        {
            var result =
                await _captchaService
                    .ValidateAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
            {
                return BadRequest(
                    result);
            }

            return Ok(
                result);
        }
    }
}
