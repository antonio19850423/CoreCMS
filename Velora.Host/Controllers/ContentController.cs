using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly ITransactionService _transactionService;

        public ContentController(IContentService ContentService)
        {

            _contentService = ContentService;
        }


        [HttpGet]
        [Route("GetSiteInfo")]
        public async Task<IActionResult> GetSiteInfo()
        {
            var result = await _contentService.GetSiteInfoAsync();
            return Ok(result);
        }



    }
}
