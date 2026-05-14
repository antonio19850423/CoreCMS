using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IGeneralSettingService : IGenericService<SqlGeneralsetting, PgGeneralsetting, GeneralSettingDto>, IBaseService
    {
        Task<GeneralSettingDto?> GetByKeyAsync(string key);
        Task<List<LocalizationtranslationDto>> GetAvailableLanguagesAsync(HttpContext httpContext);
        Task<string> GetCurrentLanguageAsync(HttpContext httpContext);
        Task<Dictionary<string,string>> GetAllTranslationsAsync(string currentLanguage);
    }
}
