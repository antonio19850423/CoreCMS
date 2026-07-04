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


        //[HttpGet]
        //[Route("GetSiteInfo")]
        //public async Task<IActionResult> GetSiteInfo()
        //{
        //    var result = await _contentService.GetSiteInfoAsync();
        //    return Ok(result);
        //}
        [HttpGet]
        [Route("GetSiteInfoAsync")]
        public async Task<ResultDto<SiteInfoDto>> GetSiteInfoAsync()
        {
            try
            {
                var footerData = await _pageService.GetFooterAsync();
                var siteData = await _contentService.GetSiteInfoAsync();

                if (!footerData.Success || !siteData.Success)
                {
                    return new ResultDto<SiteInfoDto>
                    {
                        Success = false,
                        Message = "Failed to load site info.",
                        Errors = new List<string>
                {
                    footerData.Message,
                    siteData.Message
                }
                    };
                }

                var result = new SiteInfoDto
                {
                    Footer = footerData.Data,
                    Settings = siteData.Data
                };

                return new ResultDto<SiteInfoDto>
                {
                    Data = result,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<SiteInfoDto>
                {
                    Success = false,
                    Message = ex.Message,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
        [HttpGet]
        [Route("GetPageAsync")]
        public async Task<IActionResult> GetPageAsync(string slug)
        {
            var result = await _pageService.GetPageAsync(slug);
            return Ok(result);
        }

        [HttpGet]
        [Route("GetContentDetailAsync")]
        public async Task<IActionResult> GetContentDetailAsync(string contentType,string slug)
        {
            var result = await _pageService.GetContentDetailAsync(contentType,slug);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetContentPageAsync")]
        public async Task<IActionResult> GetContentPageAsync(string slug,
            int page = 1,
            int pageSize = 10,
            string? categorySlug = null,
            string? search = null,
            string? contentType = null,
            string sort = "newest")
        {
            var result = await _pageService.GetContentPageAsync(slug,page,pageSize,categorySlug,search,contentType,sort);
            return Ok(result);
        }

    }
}
