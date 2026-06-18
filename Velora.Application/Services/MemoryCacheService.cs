using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class MemoryCacheService<TEntitySql, TEntityPg, TDto> : IMemoryCacheService<TDto>
        where TEntitySql : class
        where TEntityPg : class
        where TDto : class
    {
        private readonly IGenericService<TEntitySql, TEntityPg, TDto> _genericService;
        private readonly IMemoryCache _cache;
        private readonly string _cacheKey;
        private readonly IWebHostEnvironment _env;

        public MemoryCacheService(
            IGenericService<TEntitySql, TEntityPg, TDto> genericService,
            IMemoryCache cache, IWebHostEnvironment env)
        {
            _genericService = genericService;
            _cache = cache;
            _cacheKey = typeof(TDto).FullName!;
            _env = env;
        }

        /// <summary>
        /// گرفتن لیست اصلی Dtoها از DB یا Cache
        /// </summary>
        public async Task<List<TDto>> GetAllAsync(bool forceRefresh = false)
        {
            if (!_cache.TryGetValue(_cacheKey, out List<TDto> data) || forceRefresh)
            {
                var result = await _genericService.GetAllAsync();
                data = result.Data?.ToList() ?? new List<TDto>();
                _cache.Set(_cacheKey, data, TimeSpan.FromHours(2));
            }
            return data;
        }

        /// <summary>
        /// گرفتن لیست Viewها (مثلا برای LocalizationViewDto) از DB یا Cache
        /// </summary>
        public async Task<List<TView>> GetAllViewAsync<TView>() where TView : class
        {
            if (!_cache.TryGetValue($"{_cacheKey}_{typeof(TView).Name}", out List<TView> data) ||
               _env.IsDevelopment())
            {
                var query = await _genericService.GetAllViewQueryable<TEntityPg, TEntitySql, TView>();
                data = query.ToList();

                if (!_env.IsDevelopment())
                    _cache.Set($"{_cacheKey}_{typeof(TView).Name}", data, TimeSpan.FromMinutes(0));
            }
            return data;
        }

        public async Task<TView?> GetFirstOrDefaultViewAsync<TView>(Expression<Func<TView, bool>> predicate) where TView : class
        {
            // کلید کش بر اساس نوع ویو
            var cacheKey = $"{_cacheKey}_{typeof(TView).Name}";

            if (!_cache.TryGetValue(cacheKey, out List<TView> data) ||
               _env.IsDevelopment())
            {
                // گرفتن query جنریک
                var query = await _genericService.GetAllViewQueryable<TEntityPg, TEntitySql, TView>();
                data = query.ToList();
                if (!_env.IsDevelopment())
                    _cache.Set(cacheKey, data, TimeSpan.FromMinutes(2));
            }

            // اعمال شرط و گرفتن اولین نتیجه
            return data.AsQueryable().FirstOrDefault(predicate);
        }


        /// <summary>
        /// ریفرش دستی کش
        /// </summary>
        public async Task RefreshCacheAsync()
        {
            var result = await _genericService.GetAllAsync();
            var data = result.Data?.ToList() ?? new List<TDto>();
            _cache.Set(_cacheKey, data, TimeSpan.FromHours(2));
        }
        public async Task RefreshViewCacheAsync<TView>() where TView : class
        {
            var query = await _genericService.GetAllViewQueryable<TEntityPg, TEntitySql, TView>();
            var data = query.ToList();
            _cache.Set($"{_cacheKey}_{typeof(TView).Name}", data, TimeSpan.FromHours(2));
        }
        public async Task<Dictionary<LocalizationKeys, LocalizationViewDto>> GetMessagesAsync(
    string languageCode,
    params LocalizationKeys[] keys)
        {
            var result = new Dictionary<LocalizationKeys, LocalizationViewDto>();

            foreach (var key in keys)
            {
                var msg = await GetFirstOrDefaultViewAsync<LocalizationViewDto>(
                    c => c.LanguageCode == languageCode && c.LocalizationKeyCode.ToUpper() == key.ToMessageKey().ToUpper()
                );
                result[key] = msg;
            }

            return result;
        }
    }


}
