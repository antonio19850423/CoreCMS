using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;

namespace Velora.Host.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService
            _userAuthenticationService;
        private readonly ICaptchaService
        _captchaService;
        private readonly IUserAddressService _userAddressService;
        private readonly ICookieService _cookieService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IShoppingCartService _shoppingCartService;
        public UserAuthenticationController(
            IUserAuthenticationService
                userAuthenticationService, ICaptchaService captchaService, IUserAddressService userAddressService, ICookieService cookieService, ICurrentUserService currentUserService, IShoppingCartService shoppingCartService)
        {
            _userAuthenticationService =
                userAuthenticationService;
            _captchaService = captchaService;
            _userAddressService = userAddressService;
            _cookieService = cookieService;
            _currentUserService = currentUserService;
            _shoppingCartService = shoppingCartService;
        }

        /// <summary>
        /// ارسال کد یک‌بارمصرف به شماره موبایل
        /// </summary>
        [AllowAnonymous]
        [HttpPost("RequestOtp")]
        public async Task<IActionResult> RequestOtp(
            [FromBody] RequestOtpDto input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<RequestOtpResultDto>
                    {
                        Success = false,
                        Message =
                            "اطلاعات درخواست ارسال نشده است."
                    });
            }
            if (input.CaptchaId != null && input.CaptchaCode != null)
            {
                var captchaResult =
        await _captchaService
            .ValidateAsync(
                new CaptchaValidateDto
                {
                    CaptchaId =
                        input.CaptchaId,

                    UserInput =
                        input.CaptchaCode
                },
                cancellationToken);

                if (!captchaResult.Success)
                {
                    throw new BusinessException(
                        captchaResult.Message
                        ??
                        "کد امنیتی صحیح نیست.");
                }
            }



            var result =
                await _userAuthenticationService
                    .RequestOtpAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// بررسی و تأیید کد یک‌بارمصرف
        /// </summary>
        [AllowAnonymous]
        [HttpPost("VerifyOtp")]
        public async Task<IActionResult> VerifyOtp(
            [FromBody] VerifyOtpDto input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<VerifyOtpResultDto>
                    {
                        Success = false,
                        Message =
                            "اطلاعات درخواست ارسال نشده است."
                    });
            }

            var result =
                await _userAuthenticationService
                    .VerifyOtpAsync(
                        input,
                        cancellationToken);
        
            if (!result.Success)
                return BadRequest(result);
            var userId = result.Data.User.Id;
            var cartToken =
                _cookieService
                .GetOrCreate(
                    CookieKeys.CartToken,
                    () => Guid.NewGuid().ToString());
                await _shoppingCartService
                .MergeAsync(
                    userId,
                    cartToken
                );

            return Ok(result);
        }

        /// <summary>
        /// تکمیل ثبت‌نام کاربر و دریافت توکن
        /// </summary>
        [HttpPost("CompleteRegistration")]
        public async Task<IActionResult> CompleteRegistration(
            [FromBody] CompleteRegistrationDto input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<LoginResultDto>
                    {
                        Success = false,
                        Message =
                            "اطلاعات ثبت‌نام ارسال نشده است."
                    });
            }

            var result =
                await _userAuthenticationService
                    .CompleteRegistrationAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// ایجاد آدرس جدید برای کاربر
        /// </summary>
        [Authorize]
        [HttpPost("CreateUserAddress")]
        public async Task<IActionResult> CreateUserAddress(
            [FromBody] UserAddressCrud input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<UserAddressDto>
                    {
                        Success = false,
                        Message =
                            "اطلاعات آدرس ارسال نشده است."
                    });
            }





            var result =
                await _userAuthenticationService
                    .CreateUserAddressAsync(
                        input,
                        cancellationToken);



            if (!result.Success)
                return BadRequest(result);


            return Ok(result);
        }

        /// <summary>
        /// حذف آدرس کاربر
        /// </summary>
        [Authorize]
        [HttpDelete("DeleteUserAddress/{addressId}")]
        public async Task<IActionResult> DeleteUserAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {





            var result =
                await _userAuthenticationService
                    .DeleteUserAddressAsync(
                        addressId,
                        cancellationToken);



            if (!result.Success)
                return BadRequest(result);



            return Ok(result);
        }

        /// <summary>
        /// ویرایش آدرس کاربر
        /// </summary>
        [Authorize]
        [HttpPut("UpdateUserAddress/{addressId}")]
        public async Task<IActionResult> UpdateUserAddress(
            Guid addressId,
            [FromBody] UserAddressCrud input,
            CancellationToken cancellationToken)
        {

            if (input == null)
            {
                return BadRequest(
                    new ResultDto<UserAddressDto>
                    {
                        Success = false,
                        Message =
                            "اطلاعات آدرس ارسال نشده است."
                    });
            }


            var result =
                await _userAuthenticationService
                    .UpdateUserAddressAsync(
                        addressId,
                        input,
                        cancellationToken);



            if (!result.Success)
                return BadRequest(result);



            return Ok(result);
        }
        /// <summary>
        /// دریافت جزئیات یک آدرس
        /// </summary>
        [Authorize]
        [HttpGet("GetUserAddress/{addressId}")]
        public async Task<IActionResult> GetUserAddress(
            Guid addressId,
            CancellationToken cancellationToken)
        {





            var result =
                await _userAddressService
                    .GetUserAddressByIdAsync(
                        addressId);



            if (!result.Success)
                return BadRequest(result);



            return Ok(result);
        }

        /// <summary>
        /// دریافت لیست آدرس‌های کاربر
        /// </summary>
        [Authorize]
        [HttpGet("GetUserAddresses")]
        public async Task<IActionResult> GetUserAddresses(
            CancellationToken cancellationToken)
        {




            var result =
                await _userAddressService
                    .GetUserAddressesAsync();



            if (!result.Success)
                return BadRequest(result);



            return Ok(result);
        }

        /// <summary>
        /// تغییر رمز عبور کاربر واردشده
        /// </summary>
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<bool>
                    {
                        Success = false,
                        Message =
                            "اطلاعات تغییر رمز عبور ارسال نشده است."
                    });
            }

            var result =
                await _userAuthenticationService
                    .ChangePasswordAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    /// <summary>
    /// تغییر ایمیل کاربر
    /// </summary>
    [Authorize]
        [HttpPut("UpdateUserEmail")]
        public async Task<IActionResult> UpdateUserEmail(
        [FromBody] UpdateUserEmailDto input,
        CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<UserDto>
                    {
                        Success = false,

                        Message =
                            "اطلاعات ایمیل ارسال نشده است."
                    });
            }

            var result =
                await _userAuthenticationService
                    .UpdateUserEmailAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        /// <summary>
        /// تغییر تصویر پروفایل کاربر
        /// </summary>
        /// <summary>
        /// تغییر تصویر پروفایل کاربر
        /// </summary>
        [Authorize]
        [HttpPut("UpdateUserProfileImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateUserProfileImage(
            [FromForm] UpdateUserProfileImageDto input,
            CancellationToken cancellationToken)
        {
            if (input == null)
            {
                return BadRequest(
                    new ResultDto<UserProfileDto>
                    {
                        Success = false,
                        Message = "اطلاعات تصویر پروفایل ارسال نشده است."
                    });
            }

            var result =
                await _userAuthenticationService
                    .UpdateUserProfileImageAsync(
                        input,
                        cancellationToken);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
