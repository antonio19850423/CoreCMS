using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid GetUserId()
        {
            var claim = _httpContextAccessor.HttpContext?.User.Claims
                        .FirstOrDefault(c => c.Type == "UserGuid");
            return claim != null ? Guid.Parse(claim.Value) : Guid.Empty;
        }

        public List<string> GetRoles()
        {

            return _httpContextAccessor.HttpContext?.User.Claims
                        .Where(c => c.Type == ClaimTypes.Role)
                        .Select(c => c.Value)
                        .ToList() ?? new List<string>();
        }

        public string GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User.Identity?.Name;
        }
    }

}
