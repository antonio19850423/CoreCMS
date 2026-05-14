using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocalizationController : ControllerBase
    {
        private readonly IMemoryCacheService<LocalizationViewDto> _localizationCacheService;

        public LocalizationController(IMemoryCacheService<LocalizationViewDto> localizationCacheService)
        {
            _localizationCacheService = localizationCacheService;
        }

        [HttpGet("GetAllView")]
        public async Task<IActionResult> GetAllView()
        {
            var data = await _localizationCacheService.GetAllViewAsync<LocalizationViewDto>();

            return Ok(data);
        }

        [HttpPost("RefreshView")]
        public async Task<IActionResult> RefreshView()
        {
            await _localizationCacheService.RefreshViewCacheAsync<LocalizationViewDto>();
            return Ok("Cache refreshed");
        }
    }
}
