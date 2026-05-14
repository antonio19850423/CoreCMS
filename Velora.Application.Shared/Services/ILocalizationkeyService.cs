using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ILocalizationkeyService : IGenericService<SqlLocalizationKey, PgLocalizationkey, LocalizationkeyDto>, IBaseService
    {
    }
}
