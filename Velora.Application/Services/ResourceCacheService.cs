using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class ResourceCacheService
    : MemoryCacheService<SqlResourcesView, PgResourcesView, ResourcesViewDto>,
      IResourceCacheService
    {
        public ResourceCacheService(
            IGenericService<SqlResourcesView, PgResourcesView, ResourcesViewDto> genericService,
            IMemoryCache cache
        ) : base(genericService, cache)
        {
        }
        public async Task<ResultDto<List<ResourcesViewDto>>> GetResourcesAsync(
    string languageCode,
    params string[] resourceCodes)
        {
            // گرفتن کل لیست از کش یا DB
            var allResources = await GetAllViewAsync<ResourcesViewDto>();
            var resourceCodesSet = new HashSet<string>(resourceCodes, StringComparer.OrdinalIgnoreCase);

            var filtered = allResources
                .Where(r => r.LanguageCode==languageCode && resourceCodes.Any(rc => r.ResourceCode != null
                                                     && (r.ResourceCode.Contains(rc, StringComparison.OrdinalIgnoreCase) || r.ResourceTypeCode.Contains(rc, StringComparison.OrdinalIgnoreCase)))).ToList();

            return new ResultDto<List<ResourcesViewDto>>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Data = filtered
            };
        }

    }
}
