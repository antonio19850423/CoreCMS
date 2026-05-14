using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ILocalizationtranslationService : IGenericService<SqlLocalizationtranslation, PgLocalizationtranslation, LocalizationtranslationDto>, IBaseService
    {
        Task<LocalizationtranslationDto?> GetByCodeAsync(string code, string languageCode);
        Task<Dictionary<string,string>> GetTranslationsByLanguageAsync(string languageCode);
    }
}
