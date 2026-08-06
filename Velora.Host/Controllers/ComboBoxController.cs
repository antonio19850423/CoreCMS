using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Host.Controllers
    {
    [ApiController]
    [Route("api/[controller]")]
    public class ComboBoxController:ControllerBase
        {
        private readonly IRoleService _roleService;
        private readonly IResourceService _resourceService;
        private readonly IResourceTypeService _resourceTypeService;
        private readonly ISectionGroupItemService _sectionGroupItemService;
        private readonly ILinkTypeService _linkTypeService;
        private readonly IPageService _pageService;
        private readonly ISiteMenuService _siteMenuService;
        private readonly IContentCategoryService _contentCategoryService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductAttributeService _productAttributeService;
        private readonly IProductBrandService _productBrandService;
        private readonly IProductTypeService _productTypeService;
        private readonly IInventoryTransactionReasonService _inventoryTransactionReasonService;
        private readonly IProductService _productService;
        private readonly IProductVariantService _productVariantService;
        private readonly ICityService _cityService;
        private readonly IStateService _stateService;

        public ComboBoxController(IRoleService roleService, IResourceTypeService ResourceTypeService, IResourceService resourceService, ISectionGroupItemService sectionGroupItemService, ILinkTypeService linkTypeService, IPageService pageService, ISiteMenuService siteMenuService, IContentCategoryService contentCategoryService, IProductCategoryService productCategoryService, IProductAttributeService productAttributeService, IProductBrandService productBrandService, IProductTypeService productTypeService, IInventoryTransactionReasonService inventoryTransactionReasonService, IProductService productService, IProductVariantService productVariantService, ICityService cityService, IStateService stateService)
            {
            _roleService = roleService;
            _resourceTypeService=ResourceTypeService;
            _resourceService = resourceService;
            _sectionGroupItemService = sectionGroupItemService;
            _linkTypeService = linkTypeService;
            _pageService = pageService;
            _siteMenuService = siteMenuService;
            _contentCategoryService = contentCategoryService;
            _productCategoryService = productCategoryService;
            _productAttributeService = productAttributeService;
            _productBrandService = productBrandService;
            _productTypeService = productTypeService;
            _inventoryTransactionReasonService = inventoryTransactionReasonService;
            _productService = productService;
            _productVariantService = productVariantService;
            _cityService = cityService;
            _stateService = stateService;
            }
        [HttpGet("roles")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> GetRoles()
            {
            var roles = await _roleService.GetAllQuery();

            var roleItems = roles
                .Select(r => new ComboBoxItemDto<Guid>
                    {
                    Value = r.Id,
                    Label = r.Name
                    })
                .ToList(); // اگر لازم باشد IEnumerable کافیست ToList حذف شود

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
                {
                Data = roleItems,
                Success = true
                };

            return result;
            }

        [HttpGet("resourceTypes")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> GetResourceTypes()
        {
            var resourceTypes = await _resourceTypeService.GetAllQuery();

            var resourceItems = resourceTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList(); // اگر لازم باشد IEnumerable کافیست ToList حذف شود

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("SectionGroupItems")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> SectionGroupItems()
        {
            return await _sectionGroupItemService.GetFooterSectionGroupItemsAsync();
        }
        [HttpGet("resources")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> GetResources()
        {
            var resources = await _resourceService.GetAllQuery();

            var resourceItems = resources
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList(); // اگر لازم باشد IEnumerable کافیست ToList حذف شود

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("permissions")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<int>>>> GetPermissions()
        {
            var values = Enum.GetValues(typeof(Application.Shared.Enums.Permission))
                 .Cast<Application.Shared.Enums.Permission>()
                 .Select(x => new { Id = x, Name = x.GetDescription() }) // <--- اصلاح شد
                 .ToList();

            var items = values
                .Select(r => new ComboBoxItemDto<int>
                {
                    Value = (int)r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<int>>>
            {
                Data = items,
                Success = true
            };

            return result;
        }
        [HttpGet("LinkTypes")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> LinkTypes()
        {

            var linkTypes = await _linkTypeService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name,
                    Code=r.Code
                })
                .ToList(); 

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }

        [HttpGet("Pages")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> Pages()
        {

            var linkTypes = await _pageService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name,
                    Code = r.Slug
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("SiteMenus")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> SiteMenus()
        {

            var linkTypes = await _siteMenuService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Link1Text
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ContentCategories")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ContentCategories()
        {

            var linkTypes = await _contentCategoryService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ProductCategories")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ProductCategories()
        {

            var linkTypes = await _productCategoryService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ProductAttributes")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ProductAttributes()
        {

            var linkTypes = await _productAttributeService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ProductBrands")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ProductBrands()
        {

            var linkTypes = await _productBrandService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id.Value,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ProductTypes")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ProductTypes()
        {

            var linkTypes = await _productTypeService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("InventoryTransactionReasons")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> InventoryTransactionReasons()
        {

            var linkTypes = await _inventoryTransactionReasonService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("Products")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> Products()
        {

            var linkTypes = await _productService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("ProductVariants")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> ProductVariants()
        {

            var linkTypes = await _productVariantService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("OperationTypes")]
        public ResultDto<IEnumerable<ComboBoxItemDto<int>>> OperationTypes()
        {
            return new ResultDto<IEnumerable<ComboBoxItemDto<int>>>
            {
                Data = EnumHelper.GetComboItems<OperationType>(),
                Success = true
            };
        }
        [HttpGet("SmsProviders")]
        public ResultDto<IEnumerable<ComboBoxItemDto<int>>> SmsProviders()
        {
            return new ResultDto<IEnumerable<ComboBoxItemDto<int>>>
            {
                Data = EnumHelper.GetComboItems<SmsProvider>(),
                Success = true
            };
        }
        [HttpGet("PaymentProviders")]
        public ResultDto<IEnumerable<ComboBoxItemDto<int>>> PaymentProviders()
        {
            return new ResultDto<IEnumerable<ComboBoxItemDto<int>>>
            {
                Data = EnumHelper.GetComboItems<PaymentProvider>(),
                Success = true
            };
        }
        [HttpGet("States")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> States()
        {

            var linkTypes = await _stateService.GetAllViews();

            var resourceItems = linkTypes
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.StateTitle
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }

        [HttpGet("Cities")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> States(Guid CityId)
        {

            var cities = await _cityService.GetCitiesByStateIdAsync(CityId);

            var resourceItems = cities.Data
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.Name
                })
                .ToList();

            var result = new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };

            return result;
        }
        [HttpGet("AllCities")]
        public async Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> AllCities()
        {
            var cities = await _cityService.GetAllViews();


            var resourceItems = cities
                .Select(r => new ComboBoxItemDto<Guid>
                {
                    Value = r.Id,
                    Label = r.CityTitle
                })
                .ToList();


            return new ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>
            {
                Data = resourceItems,
                Success = true
            };
        }
    }
}
