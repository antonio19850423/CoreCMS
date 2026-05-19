using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class LocalizationCacheService
    : MemoryCacheService<SqlLocalizationView, PgLocalizationView, LocalizationViewDto>,
      ILocalizationCacheService
    {
        public LocalizationCacheService(
            IGenericService<SqlLocalizationView, PgLocalizationView, LocalizationViewDto> genericService,
            IMemoryCache cache,
            IWebHostEnvironment env
        ) : base(genericService, cache,env)
        {
        }
    }
}
