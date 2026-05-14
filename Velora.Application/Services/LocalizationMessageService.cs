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
    public class LocalizationMessageService:ILocalizationMessageService
        {
        private readonly IGeneralContextService _generalContextService;
        private readonly IMemoryCacheService<LocalizationViewDto> _localizationCacheService;

        public LocalizationMessageService(
            IGeneralContextService generalContextService,
            IMemoryCacheService<LocalizationViewDto> localizationCacheService)
            {
            _generalContextService = generalContextService;
            _localizationCacheService = localizationCacheService;
            }

        public async Task<(string successMessage, string errorMessage)> GetSaveMessagesAsync()
            {
            var messages = await _localizationCacheService.GetMessagesAsync(
                _generalContextService.CurrentLanguage,
                LocalizationKeys.SaveSuccess,
                LocalizationKeys.ServerError
            );

            return (
                messages[LocalizationKeys.SaveSuccess]?.Value ?? "SaveSuccess",
                messages[LocalizationKeys.ServerError]?.Value ?? "ServerError"
            );
            }

        public async Task<(string successMessage, string errorMessage)> GetUpdateMessagesAsync()
            {
            var messages = await _localizationCacheService.GetMessagesAsync(
                _generalContextService.CurrentLanguage,
                LocalizationKeys.UpdateSuccess,
                LocalizationKeys.ServerError
            );

            return (
                messages[LocalizationKeys.UpdateSuccess]?.Value ?? "UpdateSuccess",
                messages[LocalizationKeys.ServerError]?.Value ?? "ServerError"
            );
            }

        public async Task<(string successMessage, string errorMessage)> GetDeleteMessagesAsync()
            {
            var messages = await _localizationCacheService.GetMessagesAsync(
                _generalContextService.CurrentLanguage,
                LocalizationKeys.DeleteSuccess,
                LocalizationKeys.ServerError
            );

            return (
                messages[LocalizationKeys.DeleteSuccess]?.Value ?? "DeleteSuccess",
                messages[LocalizationKeys.ServerError]?.Value ?? "ServerError"
            );
            }

        // 🔹 متد dynamic / دلخواه
        public async Task<string> GetMessageAsync(
            LocalizationKeys key,
            string defaultValue = "",
            params object[] args)
            {
            var messages = await _localizationCacheService.GetMessagesAsync(
                _generalContextService.CurrentLanguage,key
            );

            var value = messages.ContainsKey(key) ? messages[key]?.Value : defaultValue;

            if(args != null && args.Length > 0)
                {
                value = string.Format(value ?? defaultValue,args);
                }

            return value ?? defaultValue;
            }

        }


    }
