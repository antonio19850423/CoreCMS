using GreenDonut;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITransactionService _transactionService;
        private readonly IGeneralSettingService _generalSettingService;
        private readonly IResourceService _resourceService;

        public AuthController(IAuthService authService, ITransactionService transactionService, IGeneralSettingService generalSettingService, IResourceService resourceService)
        {
            _authService = authService;
            _transactionService = transactionService;
            _generalSettingService = generalSettingService;
            _resourceService = resourceService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var user = await _authService.RegisterAsync(registerDto);
           await _transactionService.CommitAsync();
            if (user == null)
                return BadRequest("Registration failed.");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            var authResult = await _authService.LoginAsync(loginDto);
            return StatusCode(authResult.StatusCode, authResult);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var authResult = await _authService.LogoutAsync();
            return Ok(new
            {
                authResult.Data
            });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var result = await _authService.RefreshTokenAsync();

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
        [HttpGet("GetSettings")]
        public async Task<IActionResult> GetSettings()
        {
            // گرفتن زبان‌ها
            var availableLanguages = await _generalSettingService.GetAvailableLanguagesAsync(HttpContext);
            var currentLanguage = await _generalSettingService.GetCurrentLanguageAsync(HttpContext);
            var translations = await _generalSettingService.GetAllTranslationsAsync(currentLanguage);
            // گرفتن منوها برای زبان فعلی
            var menus = await _resourceService.GetAllMenusAsync(currentLanguage);
            // بازگشت داده‌ها
            return Ok(new
            {
                AvailableLanguages = availableLanguages,
                CurrentLanguage = currentLanguage,
                Menus = menus,
                Translations = translations
            });
        }

    }

}
