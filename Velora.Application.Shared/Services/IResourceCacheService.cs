using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IResourceCacheService : IMemoryCacheService<ResourcesViewDto>
    {
        Task<ResultDto<List<ResourcesViewDto>>> GetResourcesAsync(
    string languageCode,
    params string[] resourceCodes);
    }

}
