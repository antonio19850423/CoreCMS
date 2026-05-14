using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public ComboBoxController(IRoleService roleService, IResourceTypeService ResourceTypeService, IResourceService resourceService)
            {
            _roleService = roleService;
            _resourceTypeService=ResourceTypeService;
            _resourceService = resourceService;
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


    }
}
