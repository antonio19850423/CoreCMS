using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;


namespace Velora.Application.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeResourceAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public string[] Roles { get; }

        public AuthorizeResourceAttribute(params string[] roles)
        {
            Roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var userRoles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => Guid.Parse(c.Value)) // ✅ تبدیل به Guid
                .ToList();

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var resourceCode = GetResourceName(actionDescriptor);
            if (string.IsNullOrEmpty(resourceCode))
            {
                context.Result = new ForbidResult();
                return;
            }

            var permissionCache = context.HttpContext.RequestServices
                .GetRequiredService<IPermissionCacheService>();

            // 🔹 بررسی دسترسی async
            var hasAccess = await permissionCache.HasAccessAsync(userRoles, resourceCode);
            if (!hasAccess)
            {
                context.Result = new ObjectResult(new ResultDto<object>
                {
                    Success = false,
                    Message = "You do not have access to this resource",
                    Errors = new List<string> { "AccessDenied" },
                    StatusCode = StatusCodes.Status403Forbidden
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }

        public static string GetResourceName(ControllerActionDescriptor actionDescriptor)
        {
            var controllerName = actionDescriptor.ControllerName;
            var actionName = actionDescriptor.ActionName;
            return $"Api.{controllerName}.{actionName}";
        }
    }

}
