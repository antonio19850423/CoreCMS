using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Services;

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

        public ComboBoxController(IRoleService roleService, IResourceTypeService ResourceTypeService, IResourceService resourceService, ISectionGroupItemService sectionGroupItemService, ILinkTypeService linkTypeService, IPageService pageService, ISiteMenuService siteMenuService, IContentCategoryService contentCategoryService)
            {
            _roleService = roleService;
            _resourceTypeService=ResourceTypeService;
            _resourceService = resourceService;
            _sectionGroupItemService = sectionGroupItemService;
            _linkTypeService = linkTypeService;
            _pageService = pageService;
            _siteMenuService = siteMenuService;
            _contentCategoryService = contentCategoryService;
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
            var values = Enum.GetValues(typeof(Permission))
                 .Cast<Permission>()
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
    }
}
