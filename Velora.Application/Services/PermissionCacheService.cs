using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class PermissionCacheService : IPermissionCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IRolePermissionService _rolePermissionService;

        private const string CacheKey = "ROLE_PERMISSION_RESOURCE_MAP";

        public PermissionCacheService(
            IMemoryCache cache,
            IRolePermissionService rolePermissionService)
        {
            _cache = cache;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<bool> HasAccessAsync(IEnumerable<Guid> userRoleIds, string resourceCode)
        {
            if (userRoleIds == null || !userRoleIds.Any() || string.IsNullOrEmpty(resourceCode))
                return false;

            // فقط نقش‌های واقعی کاربر
            var map = await BuildMapAsync(userRoleIds);

            foreach (var roleId in userRoleIds)
            {
                if (map.TryGetValue(roleId, out var resources))
                {
                    if (resources.Contains(resourceCode, StringComparer.OrdinalIgnoreCase))
                        return true;
                }
            }

            return false;
        }



        private async Task<Dictionary<Guid, HashSet<string>>> BuildMapAsync(IEnumerable<Guid> filterRoleIds)
        {
            var permissions = await _rolePermissionService.GetRolePermissionMapAsync();

            // فیلتر بر اساس نقش‌های واقعی کاربر
            permissions = permissions
                .Where(p => filterRoleIds.Contains(p.RoleId))
                .ToList();

            return permissions.ToDictionary(
                x => x.RoleId,
                x => x.ResourceCodes.ToHashSet(StringComparer.OrdinalIgnoreCase)
            );
        }


        public async Task RefreshAsync()
        {
            // دریافت همه نقش‌ها و Resourceها بدون فیلتر
            var permissions = await _rolePermissionService.GetRolePermissionMapAsync();

            var map = permissions.ToDictionary(
                x => x.RoleId,
                x => x.ResourceCodes.ToHashSet(StringComparer.OrdinalIgnoreCase)
            );

            _cache.Set(CacheKey, map, TimeSpan.FromMinutes(30));
        }

    }

}
