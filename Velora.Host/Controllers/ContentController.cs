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
        private readonly IPageService _pageService;


        public ContentController(IContentService ContentService, IPageService pageService)
        {

            _contentService = ContentService;
            _pageService = pageService;
        }


        [HttpGet]
        [Route("GetSiteInfo")]
        public async Task<IActionResult> GetSiteInfo()
        {
            var result = await _contentService.GetSiteInfoAsync();
            return Ok(result);
        }
        [HttpGet]
        [Route("GetPageAsync")]
        public async Task<IActionResult> GetPageAsync(string slug)
        {
            var result = await _pageService.GetPageAsync(slug);
            return Ok(result);
        }



    }
}
