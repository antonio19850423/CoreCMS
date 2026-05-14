using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IGeneralContextService _generalContextService;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserRoleService _userRoleService;
        private readonly IRoleService _roleService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;
        protected readonly DatabaseType _dbType;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCacheService<LocalizationViewDto> _localizationCacheService;
        private readonly JwtSettings _jwtSettings;
        protected readonly ICurrentUserService _currentUserService;
        public AuthService(
            IUserService userService,
            IUserProfileService userProfileService,
            IUserRoleService userRoleService,
            IRoleService roleService,
            IJwtTokenService jwtTokenService,
            IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IOptions<JwtSettings> jwtSettings,
            IGeneralContextService generalContextService, IMemoryCacheService<LocalizationViewDto> localizationCacheService, ICurrentUserService currentUserService)
        {
            _userService = userService;
            _userProfileService = userProfileService;
            _userRoleService = userRoleService;
            _roleService = roleService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
            // خواندن نوع دیتابیس از appsettings
            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
            _httpContextAccessor = httpContextAccessor;
            _generalContextService = generalContextService;
            _localizationCacheService = localizationCacheService;
            _jwtSettings = jwtSettings.Value;
            _currentUserService = currentUserService;
        }

        public async Task<ResultDto<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            var existingUser = _dbType == DatabaseType.SqlServer
    ? await _userService.FirstOrDefaultAsync<SqlUser>(x => x.UserName == registerDto.UserName)
    : await _userService.FirstOrDefaultAsync<PgUser>(x => x.UserName == registerDto.UserName);
            if (existingUser.Data != null)
            {
                return new ResultDto<UserDto>
                {
                    Success = false,
                    Message = "کاربر با این نام کاربری قبلاً ثبت‌نام کرده است",
                    Errors = new List<string> { "DuplicateUserName" }
                };
            }
            var defaultRole = _dbType == DatabaseType.SqlServer
                    ? await _roleService.FirstOrDefaultAsync<SqlRole>(x => x.Code == registerDto.DefaultRoleCode)
                    : await _roleService.FirstOrDefaultAsync<PgRole>(x => x.Code == registerDto.DefaultRoleCode);
            if (defaultRole.Data == null)
            {
                return new ResultDto<UserDto>
                {
                    Success = false,
                    Message = "نقش کاربری صحیح نمی‌باشد",
                    Errors = new List<string> { "InvalidRole" }
                };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            var userDto = new UserDto
            {
                UserName = registerDto.UserName,
                Password = hashedPassword,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true
            };

            var newUser = await _userService.CreateAsync(userDto);

            await _userRoleService.CreateAsync(new UserRoleDto
            {
                Userid = newUser.Data.Id,
                Roleid = defaultRole.Data.Id
            });

            return new ResultDto<UserDto>
            {
                Success = true,
                Message = "ثبت‌نام با موفقیت انجام شد",
                Data = newUser.Data
            };
        }


        public async Task<ResultDto<LoginResultDto>> LoginAsync(LoginDto loginDto)
        {
            // 🔹 بررسی کاربر موجود
            var existingUser = _dbType == DatabaseType.SqlServer
                ? await _userService.FirstOrDefaultAsync<SqlUser>(x => x.UserName == loginDto.UserName)
                : await _userService.FirstOrDefaultAsync<PgUser>(x => x.UserName == loginDto.UserName);

            var messages = await _localizationCacheService.GetMessagesAsync(
                _generalContextService.CurrentLanguage,
                LocalizationKeys.LoginSuccess,
                LocalizationKeys.UserNotFound,
                LocalizationKeys.InvalidPassword,
                LocalizationKeys.Unauthorized
            );

            if (existingUser.Data == null)
            {
                return new ResultDto<LoginResultDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = messages[LocalizationKeys.UserNotFound]?.Value ?? "User not found",
                    Errors = new List<string> { "UserNotFound" }
                };
            }

            // 🔹 بررسی پسورد
            if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, existingUser.Data.Password))
            {
                return new ResultDto<LoginResultDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = messages[LocalizationKeys.InvalidPassword]?.Value ?? "Invalid password",
                    Errors = new List<string> { "InvalidPassword" }
                };
            }

            // 🔹 گرفتن نقش‌ها
            var roles = await _userRoleService.GetRolesByUserIdAsync(existingUser.Data.Id);
            if (roles.Any(r => r.Id != Guid.Empty && r.Id != null))
            {
                existingUser.Data.Roles = roles;
            }

            // 🔹 تولید AccessToken و RefreshToken
            // 🔹 تولید AccessToken و RefreshToken واقعی (JWT)
            var accessToken = _jwtTokenService.GenerateToken(existingUser.Data);
            var refreshToken = _jwtTokenService.GenerateRefreshToken(existingUser.Data); // الان یک JWT واقعی است

            // 🔹 ست کردن کوکی‌ها
            SetAccessTokenCookie(accessToken.Token, accessToken.ExpireDate);
            SetRefreshTokenCookie(refreshToken.Token, refreshToken.ExpireDate); // از JwtSettings.RefreshTokenExpiryDays استفاده می‌شود

            // 🔹 چک امنیتی
            if (accessToken == null || string.IsNullOrEmpty(accessToken.Token))
            {
                return new ResultDto<LoginResultDto>
                {
                    StatusCode = StatusCodes.Status401Unauthorized,
                    Success = false,
                    Message = messages[LocalizationKeys.Unauthorized]?.Value ?? "Unauthorized",
                    Errors = new List<string> { "Unauthorized" }
                };
            }
            // 🔹 بازگشت نتیجه
            return new ResultDto<LoginResultDto>
            {
                StatusCode = StatusCodes.Status200OK,
                Success = true,
                Message = messages[LocalizationKeys.LoginSuccess]?.Value ?? "Login successful",
                Data = new LoginResultDto
                {
                    User = existingUser.Data,
                    Token = accessToken.Token,
                    ExpireDate = accessToken.ExpireDate,
                    RefreshToken = refreshToken.Token
                }
            };

        }



        public async Task<ResultDto<UserProfileDto>> CompleteProfileAsync(Guid userId, CompleteProfileDto profileDto)
        {
            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("کاربر پیدا نشد");

            var userProfileDto = _mapper.Map<UserProfileDto>(profileDto);
            userProfileDto.Userid = userId;

            var createdProfile = await _userProfileService.CreateAsync(userProfileDto);
            return createdProfile;
        }
        public async Task<ResultDto<LoginResultDto>> RefreshTokenAsync()
        {
            var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return new ResultDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Refresh token not found",
                    Errors = new List<string> { "NoRefreshToken" }
                };
            }

            var principal = _jwtTokenService.ValidateToken(refreshToken);
            if (principal == null)
            {
                return new ResultDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Invalid refresh token",
                    Errors = new List<string> { "InvalidRefreshToken" }
                };
            }

            var userIdClaim = principal.FindFirst("UserGuid")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return new ResultDto<LoginResultDto>
                {
                    Success = false,
                    Message = "Invalid token claims",
                    Errors = new List<string> { "InvalidTokenClaims" }
                };
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user.Data == null)
            {
                return new ResultDto<LoginResultDto>
                {
                    Success = false,
                    Message = "User not found",
                    Errors = new List<string> { "UserNotFound" }
                };
            }

            var roles = await _userRoleService.GetRolesByUserIdAsync(user.Data.Id);
            if (roles.Any())
                user.Data.Roles = roles;

            // ✅ تولید AccessToken جدید
            var accessToken = _jwtTokenService.GenerateToken(user.Data);

            // ✅ تولید RefreshToken جدید (JWT واقعی)
            var newRefreshToken = _jwtTokenService.GenerateRefreshToken(user.Data);

            // ✅ ست کردن کوکی RefreshToken جدید با زمان انقضای خود JWT
            SetRefreshTokenCookie(newRefreshToken.Token, newRefreshToken.ExpireDate);


            // 🔹 بازگشت نتیجه به فرانت
            return new ResultDto<LoginResultDto>
            {
                Success = true,
                Message = "Token refreshed successfully",
                Data = new LoginResultDto
                {
                    User = user.Data,
                    Token = accessToken.Token,          // AccessToken برای فرانت
                    ExpireDate = accessToken.ExpireDate,
                    RefreshToken = newRefreshToken.Token   // فرانت از آن برای درخواست refresh بعدی استفاده می‌کند
                }
            };

        }

        private void SetAccessTokenCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("accessToken", token, cookieOptions);
        }
        private void SetRefreshTokenCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        public async Task<ResultDto<bool>> LogoutAsync()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    Message = "HttpContext not available",
                    Errors = new List<string> { "NoHttpContext" }
                };
            }

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,       // اگر https دارید
                SameSite = SameSiteMode.Strict,
                Path = "/"           // حتماً همان path که کوکی ست شده
            };

            // حذف refreshToken و accessToken
            context.Response.Cookies.Delete("refreshToken", cookieOptions);
            context.Response.Cookies.Delete("accessToken", cookieOptions);

            return new ResultDto<bool>
            {
                Success = true,
                Message = "خروج از حساب کاربری با موفقیت انجام شد",
                Data = true
            };
        }






    }

}
