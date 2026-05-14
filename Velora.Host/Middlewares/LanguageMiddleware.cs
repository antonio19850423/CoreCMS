using Velora.Application.Shared.Services;

namespace Velora.Host.Middlewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IGeneralSettingService generalSettingService)
        {
            var currentLanguage = await generalSettingService.GetCurrentLanguageAsync(context);
            context.Items["CurrentLanguage"] = currentLanguage ?? "en"; // مقدار پیش‌فرض

            await _next(context);
        }
    }

}
