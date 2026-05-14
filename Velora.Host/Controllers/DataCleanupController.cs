using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataCleanupController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IUserProfileService _userProfileService;
        private readonly IResourceService _resourceService;
        private readonly IResourceTypeService _resourceTypeService;

        private readonly ITransactionService _transactionService;

        public DataCleanupController(
            IUserRoleService userRoleService,
            IUserService userService,
            IRoleService roleService,
            IUserProfileService userProfileService,
            ITransactionService transactionService,
            IResourceService resourceService,
            IResourceTypeService resourceTypeService)
        {
            _userRoleService = userRoleService;
            _userService = userService;
            _roleService = roleService;
            _userProfileService = userProfileService;
            _transactionService = transactionService;
            _resourceService = resourceService;
            _resourceTypeService = resourceTypeService;
        }

        [HttpDelete("CleanupTestData")]
        public async Task<IActionResult> CleanupTestData()
        {
            // پاکسازی UserRoles
            var userRoles = await _userRoleService.GetAllAsync();
            foreach (var ur in userRoles.Data.Where(x => x.IsTest))
            {
                await _userRoleService.DeleteAsync(ur.Id);
            }
            // پاکسازی _resourceTypeService
            var resourceTypes = await _resourceTypeService.GetAllAsync();
            foreach (var ur in resourceTypes.Data.Where(x => x.IsTest))
            {
                await _resourceTypeService.DeleteAsync(ur.Id);
            }
            // پاکسازی _resourceTypeService
            var resources = await _resourceService.GetAllAsync();
            foreach (var ur in resources.Data.Where(x => x.IsTest))
            {
                await _resourceService.DeleteAsync(ur.Id);
            }
            // پاکسازی Users
            var users = await _userService.GetAllAsync();
            foreach (var user in users.Data.Where(x => x.IsTest))
            {
                await _userService.DeleteAsync(user.Id);
            }

            // پاکسازی Roles
            var roles = await _roleService.GetAllAsync();
            foreach (var role in roles.Data.Where(x => x.IsTest))
            {
                await _roleService.DeleteAsync(role.Id);
            }

            // پاکسازی UserProfiles
            var profiles = await _userProfileService.GetAllAsync();
            foreach (var profile in profiles.Data.Where(x => x.IsTest))
            {
                await _userProfileService.DeleteAsync(profile.Id);
            }

            await _transactionService.CommitAsync();

            return Ok(new { Message = "All test data deleted successfully." });
        }

    }
}
