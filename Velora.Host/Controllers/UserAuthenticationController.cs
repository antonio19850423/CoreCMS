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
    [AllowAnonymous]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IUserAuthenticationService
            _userAuthenticationService;
        private readonly ICaptchaService
        _captchaService;
        private readonly IUserAddressService _userAddressService;
        public UserAuthenticationController(
            IUserAuthenticationService
                userAuthenticationService, ICaptchaService captchaService, IUserAddressService userAddressService)
        {
            _userAuthenticationService =
                userAuthenticationService;
            _captchaService = captchaService;
            _userAddressService = userAddressService;
        }

        /// <summary>
        /// ارسال کد یک‌بارمصرف به شماره موبایل
        /// </summary>
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
    }
}
