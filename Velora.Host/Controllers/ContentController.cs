using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Infrastructure;
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
        private readonly IContactService _contactService;
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductReviewService _productReviewService;
        private readonly IProductQuestionService _productQuestionService;
        



        public ContentController(IContentService ContentService, IPageService pageService, IContactService contactService, IProductService productService, IProductCategoryService productCategoryService, IProductBrandService productBrandService, IProductReviewService productReviewService, IProductQuestionService productQuestionService)
        {

            _contentService = ContentService;
            _pageService = pageService;
            _contactService = contactService;
            _productService= productService;
            _productCategoryService= productCategoryService;
            _productBrandService= productBrandService;
            _productReviewService = productReviewService;
            _productQuestionService= productQuestionService;
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
        [HttpPost]
        [Route("SendContactAsync")]
        public async Task<IActionResult> SendContactAsync([FromBody] ContactUsDto input)
        {
            if (!ModelState.IsValid)
            {
                return Ok(new ResultDto<ContactUsDto>
                {
                    Success = false,
                    Message = "اطلاعات ورودی معتبر نیست."
                });
            }

            var result = await _contactService.SendContactAsync(input);

            return Ok(result);
        }

        [HttpGet]
        [Route("GetProductsAsync")]
        public async Task<IActionResult> GetProductsAsync(
    int page = 1,
    int pageSize = 12,
    string? categorySlug = null,
    string? brandSlug = null,
    string? search = null,
    string sort = "newest",
    decimal? minPrice = null,
    decimal? maxPrice = null)
        {

            var result =
                await _productService.GetProductsAsync(
                    page,
                    pageSize,
                    categorySlug,
                    brandSlug,
                    search,
                    sort,
                    minPrice,
                    maxPrice
                );


            return Ok(result);

        }
        [HttpGet]
        [Route("GetProductDetailAsync")]
        public async Task<IActionResult> GetProductDetailAsync(
    string slug)
        {
            var result =
                await _productService
                    .GetProductDetailAsync(slug);


            return Ok(result);
        }
        [HttpGet]
        [Route("GetProductCategoryTreeAsync")]
        public async Task<IActionResult> GetProductCategoryTreeAsync()
        {
            var result =
                await _productCategoryService
                .GetProductCategoryTreeAsync();


            var data =
                result
                .Select(MapCategory)
                .ToList();


            return Ok(new ResultDto<List<ComboBoxItemDto<string>>>
            {
                Success = true,
                Data = data
            });
        }
        private ComboBoxItemDto<string> MapCategory(ProductCategoryTreeDto item)
        {
            return new ComboBoxItemDto<string>
            {
                Value = item.Slug,
                Label = item.Name,
                Code = item.Slug,

                Children = item.Children?
                    .Select(MapCategory)
                    .ToList()
                    ?? new()
            };
        }
        [HttpGet]
        [Route("GetProductBrandsAsync")]
        public async Task<IActionResult> GetProductBrandsAsync()
        {
            var result =
                await _productBrandService
                .GetProductBrandsAsync();


            var data =
                result.Data
                .Select(x => new ComboBoxItemDto<string>
                {
                    Value = x.Slug,
                    Label = x.Name,
                    Code = x.Slug
                })
                .ToList();


            return Ok(new ResultDto<List<ComboBoxItemDto<string>>>
            {
                Success = true,
                Data = data
            });
        }


        [Authorize]
        [HttpPost]
        [Route("AddProductReviewsync")]
        public async Task<IActionResult> AddProductReviewsync([FromBody] CreateProductReviewDto input)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = "اطلاعات نظر ارسال نشده است."
                    });
            }

            try
            {
                var result =
                    await _productReviewService.CreateUserReviewAsync(input);

                if (!result.Success)
                    return BadRequest(result);

                return Ok(new ResultDto<ProductReviewDto>
                {
                    Success = true,
                    Message = "نظر شما با موفقیت ثبت شد. پس از بررسی کارشناسان، نمایش داده خواهد شد."
                });
            }
            catch (BusinessException ex)
            {
                return BadRequest(
                    new ResultDto<ProductReviewDto>
                    {
                        Success = false,
                        Message = ex.Message
                    });
            }
        }

        [HttpGet]
        [Route("GetRatingSummaryAsync")]
        public async Task<IActionResult> GetRatingSummaryAsync(Guid productId)
        {
            var result =
                await _productReviewService
                .GetRatingSummaryAsync(productId);
            return Ok(result);
        }
        [HttpGet]
        [Route("GetUserReviewsAsync")]
        public async Task<IActionResult> GetUserReviewsAsync(Guid productId,int page = 1,int pageSize = 12)
        {
            var result =
                await _productReviewService
                .GetUserReviewsAsync(productId,page,pageSize);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("AddProductQuestionAsync")]
        public async Task<IActionResult> AddProductQuestionAsync([FromBody] CreateProductQuestionDto input)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<ProductQuestionDto>
                    {
                        Success = false,

                        Message =
                            "اطلاعات سؤال ارسال نشده است."
                    });
            }


            try
            {
                var result =
                    await _productQuestionService
                        .CreateUserQuestionAsync(input);


                if (!result.Success)
                {
                    return BadRequest(result);
                }


                return Ok(
                    new ResultDto<ProductQuestionDto>
                    {
                        Success = true,

                        Message =
                            "سؤال شما با موفقیت ثبت شد. پس از بررسی کارشناسان، نمایش داده خواهد شد."
                    });
            }
            catch (BusinessException ex)
            {
                return BadRequest(
                    new ResultDto<ProductQuestionDto>
                    {
                        Success = false,

                        Message = ex.Message
                    });
            }
        }

        [HttpGet]
        [Route("GetUserQuestionsAsync")]
        public async Task<IActionResult> GetUserQuestionsAsync(
    Guid productId,
    int page = 1,
    int pageSize = 10)
        {
            var result =
                await _productQuestionService
                    .GetUserQuestionsAsync(
                        productId,
                        page,
                        pageSize);


            return Ok(result);
        }
    }
}
