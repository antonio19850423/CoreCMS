using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;

namespace Velora.Application.Shared.Services
{
    public interface IMemoryCacheService<TDto> : IBaseService where TDto : class
    {
        Task<List<TDto>> GetAllAsync(bool forceRefresh = false);
        Task<List<TView>> GetAllViewAsync<TView>() where TView : class;
        Task RefreshCacheAsync();
        Task RefreshViewCacheAsync<TView>() where TView : class;
        Task<TView?> GetFirstOrDefaultViewAsync<TView>(Expression<Func<TView, bool>> predicate) where TView : class;
        Task<Dictionary<LocalizationKeys, LocalizationViewDto>> GetMessagesAsync(
    string languageCode,
    params LocalizationKeys[] keys);
    }
}
