using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface IGeneralContextService:IBaseService
    {
        string CurrentLanguage { get; }
        // جلوتر می‌تونی اینها هم اضافه کنی
        // string TimeZone { get; }
    }


}
