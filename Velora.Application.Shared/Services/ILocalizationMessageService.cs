using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Enums;

namespace Velora.Application.Shared.Services
    {
    public interface ILocalizationMessageService:IBaseService
        {
        // پیام‌های پیش‌فرض CRUD
        Task<(string successMessage, string errorMessage)> GetSaveMessagesAsync();
        Task<(string successMessage, string errorMessage)> GetUpdateMessagesAsync();
        Task<(string successMessage, string errorMessage)> GetDeleteMessagesAsync();

        // 🔹 پیام دلخواه / dynamic
        Task<string> GetMessageAsync(LocalizationKeys key,string defaultValue = "",params object[] args);
        }


    }
