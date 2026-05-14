using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class GeneralContextService:IGeneralContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GeneralContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string CurrentLanguage =>
            _httpContextAccessor.HttpContext?.Items["CurrentLanguage"]?.ToString() ?? "en";
    }
}
