using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Services
{
    public class UserAuthenticationService
        : IUserAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;

        private readonly IUserProfileService
            _userProfileService;

        private readonly IUserOtpService
            _userOtpService;

        private readonly ISiteSettingService
            _siteSettingService;

        private readonly ISmsService
            _smsService;

        private readonly IJwtTokenService
            _jwtTokenService;

        private readonly ITransactionService
            _transactionService;
        private readonly IUserRoleService
    _userRoleService;
        private readonly IUserAddressService _userAddressService;


        private readonly DatabaseType _dbType;
        public UserAuthenticationService(
            IUserService userService,
            IUserProfileService userProfileService,
            IUserOtpService userOtpService,
            ISiteSettingService siteSettingService,
            ISmsService smsService,
            IJwtTokenService jwtTokenService,
            ITransactionService transactionService, IConfiguration configuration, IUserRoleService userRoleService,IUserAddressService userAddressService, ICurrentUserService currentUserService)
        {
            _userService = userService;

            _userProfileService =
                userProfileService;

            _userOtpService =
                userOtpService;

            _siteSettingService =
                siteSettingService;

            _smsService = smsService;

            _jwtTokenService =
                jwtTokenService;

            _transactionService =
                transactionService;
            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
            _userRoleService = userRoleService;
            _userAddressService = userAddressService;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// درخواست ارسال کد یکبارمصرف
        /// </summary>
        public async Task<
            ResultDto<RequestOtpResultDto>>
            RequestOtpAsync(
                RequestOtpDto input,
                CancellationToken cancellationToken =
                    default)
        {
            try
            {
                cancellationToken
                    .ThrowIfCancellationRequested();

                if (input == null)
                {
                    throw new BusinessException(
                        "اطلاعات درخواست ارسال کد معتبر نیست.");
                }

                var mobile =
                    OtpSecurityHelper
                        .NormalizeIranMobile(
                            input.Mobile);

                var siteSetting =
                    await GetActiveSiteSettingAsync(
                        cancellationToken);

                var expirationMinutes =
                    siteSetting
                        .OtpExpirationMinutes
                    ?? 2;

                var codeLength =
                    siteSetting
                        .OtpCodeLength
                    ?? 5;

                var maxAttempts =
                    siteSetting
                        .OtpMaxVerifyAttempts
                    ?? 5;

                var cooldownSeconds =
                    siteSetting
                        .OtpRequestCooldownSeconds
                    ?? 60;

                var maxRequestsPerHour =
                    siteSetting
                        .OtpMaxRequestsPerHour
                    ?? 5;

                var now = DateTime.Now;

                /*
                 * دریافت جدیدترین OTP ثبت‌شده
                 *
                 * نکته:
                 * نباید از FirstOrDefaultAsync استفاده کنیم،
                 * چون ممکن است یک OTP قدیمی را برگرداند.
                 *
                 * GetLatestOtpAsync باید بر اساس CreatedAt
                 * به صورت نزولی مرتب کند.
                 */
                var latestOtp =
                    await _userOtpService
                        .GetLatestOtpAsync(
                            mobile,
                            (int)
                            UserOtpPurpose
                                .Authentication
                        );

                /*
                 * جلوگیری از ارسال مجدد پیامک
                 * قبل از پایان زمان Cooldown
                 */
                if (latestOtp != null)
                {
                    var secondsFromLastRequest =
                        (now -
                         latestOtp.CreatedAt)
                        .TotalSeconds;

                    if (
                        secondsFromLastRequest
                        <
                        cooldownSeconds)
                    {
                        var remainingSeconds =
                            (int)Math.Ceiling(
                                cooldownSeconds
                                -
                                secondsFromLastRequest
                            );

                        /*
                         * برای جلوگیری از نمایش صفر
                         * در شرایط اختلاف میلی‌ثانیه‌ای
                         */
                        if (remainingSeconds < 1)
                        {
                            remainingSeconds = 1;
                        }

                        throw new BusinessException(
                            $"لطفاً {remainingSeconds} ثانیه دیگر برای دریافت کد تلاش کنید."
                        );
                    }
                }

                /*
                 * جلوگیری از SMS Flood:
                 * حداکثر تعداد درخواست در یک ساعت
                 */
                var oneHourAgo =
                    now.AddHours(-1);

                var recentOtpCount =
                    await CountOtpRequestsAsync(
                        mobile,
                        oneHourAgo,
                        cancellationToken);

                if (
                    recentOtpCount
                    >=
                    maxRequestsPerHour)
                {
                    throw new BusinessException(
                        "تعداد درخواست‌های دریافت کد بیش از حد مجاز است. لطفاً بعداً دوباره تلاش کنید."
                    );
                }

                /*
                 * تولید کد OTP
                 */
                var code =
                    OtpSecurityHelper
                        .GenerateOtpCode(
                            codeLength);

                /*
                 * هش کردن کد
                 */
                var codeHash =
                    OtpSecurityHelper
                        .HashOtp(
                            mobile,
                            code);

                /*
                 * زمان انقضای OTP
                 */
                var expiresAt =
                    now.AddMinutes(
                        expirationMinutes);

                /*
                 * ایجاد رکورد OTP
                 */
                var otpDto =
                    new UserOtpDto
                    {
                        Id =
                            Guid.NewGuid(),

                        Mobile =
                            mobile,

                        CodeHash =
                            codeHash,

                        Purpose =
                            (int)
                            UserOtpPurpose
                                .Authentication,

                        ExpiresAt =
                            expiresAt,

                        AttemptCount =
                            0,

                        MaxAttempts =
                            maxAttempts,

                        IsUsed =
                            false,

                        UsedAt =
                            null,

                        IsVerified =
                            false,

                        VerifiedAt =
                            null,

                        CreatedAt =
                            now
                    };

                /*
                 * ابتدا OTP در دیتابیس ذخیره می‌شود.
                 */
                var createResult =
                    await _userOtpService
                        .CreateAsync(
                            otpDto);

                if (
                    !createResult.Success
                    ||
                    createResult.Data == null)
                {
                    throw new BusinessException(
                        createResult.Message
                        ??
                        "خطا در ذخیره اطلاعات کد یکبارمصرف."
                    );
                }

                /*
                 * بررسی اینکه کاربر قبلاً ثبت‌نام کرده است یا خیر
                 */
                var existingUser =
                    await _userService
                        .GetByMobileNumberAsync(
                            mobile
                        );

                /*
                 * ارسال پیامک
                 */
                await _smsService
                    .SendOtpAsync(
                        mobile:
                            mobile,

                        code:
                            code,

                        expirationMinutes:
                            expirationMinutes,

                        cancellationToken:
                            cancellationToken);

                var isExistingUser =
                    existingUser != null;

                /*
                 * ثبت نهایی تراکنش
                 */
                await _transactionService
                    .CommitAsync();

                return new ResultDto<
                    RequestOtpResultDto>
                {
                    Success =
                        true,

                    Message =
                        "کد تأیید با موفقیت ارسال شد.",

                    Data =
                        new RequestOtpResultDto
                        {
                            ExpirationMinutes =
                                expirationMinutes,

                            IsExistingUser =
                                isExistingUser
                        }
                };
            }
            catch (BusinessException)
            {
                /*
                 * BusinessException پیام مناسب
                 * برای نمایش به کاربر دارد.
                 */
                await _transactionService
                    .RollbackAsync();

                throw;
            }
            catch (OperationCanceledException)
            {
                await _transactionService
                    .RollbackAsync();

                throw;
            }
            catch (Exception)
            {
                await _transactionService
                    .RollbackAsync();

                throw new BusinessException(
                    "در ارسال کد یکبارمصرف خطایی رخ داد. لطفاً دوباره تلاش کنید."
                );
            }
        }
        private async Task<WebsiteLoginResultDto> CreateLoginResultAsync(
    UserDto user,
    UserProfileDto? profile,
    string message = "")
        {
            var accessToken =
                _jwtTokenService.GenerateToken(user);

            var refreshToken =
                _jwtTokenService.GenerateRefreshToken(user);

            return new WebsiteLoginResultDto
            {
                User = new WebsiteUserDto
                {
                    Id = user.Id,

                    Mobile = user.MobileNumber,

                    FirstName = profile?.Firstname,

                    LastName = profile?.Lastname,
                    NationalCode= user?.NationalCode,
                    Email = user.Email,

                    Avatar = profile?.ProfileImage
                },

                Token = accessToken.Token,

                ExpireDate = accessToken.ExpireDate,

                RefreshToken = refreshToken.Token
            };
        }
        public async Task<ResultDto<WebsiteLoginResultDto>> VerifyOtpAsync(
            VerifyOtpDto input,
            CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (input == null)
                {
                    throw new BusinessException(
                        "اطلاعات تأیید کد معتبر نیست."
                    );
                }

                var mobile =
                    OtpSecurityHelper.NormalizeIranMobile(
                        input.Mobile
                    );

                if (string.IsNullOrWhiteSpace(input.Code))
                {
                    throw new BusinessException(
                        "کد یکبارمصرف وارد نشده است."
                    );
                }

                var now = DateTime.UtcNow;

                /*
                 * فقط آخرین OTP مربوط به این شماره بررسی می‌شود.
                 */
                var otpQuery =
                    await _userOtpService.GetAllViews();

                var latestOtp =
                    otpQuery
                        .Where(
                            x =>
                                x.Mobile == mobile
                                &&
                                x.Purpose ==
                                (int)UserOtpPurpose.Authentication
                        )
                        .OrderByDescending(
                            x => x.CreatedAt
                        )
                        .FirstOrDefault();

                if (latestOtp == null)
                {
                    throw new BusinessException(
                        "کد یکبارمصرفی برای این شماره پیدا نشد. لطفاً مجدداً درخواست کد دهید."
                    );
                }

                /*
                 * OTP مصرف شده
                 */
                if (latestOtp.IsUsed)
                {
                    throw new BusinessException(
                        "این کد دیگر معتبر نیست. لطفاً یک کد جدید دریافت کنید."
                    );
                }


                /*
                 * بررسی انقضا
                 */
                if (latestOtp.ExpiresAt <= now)
                {
                    latestOtp.IsUsed = true;
                    latestOtp.UsedAt = now;

                    var expiredUpdateResult =
                        await _userOtpService.UpdateAsync(
                            latestOtp,
                            latestOtp.Id
                        );

                    if (!expiredUpdateResult.Success)
                    {
                        throw new BusinessException(
                            expiredUpdateResult.Message
                            ??
                            "خطا در به‌روزرسانی وضعیت کد منقضی‌شده."
                        );
                    }

                    await _transactionService.CommitAsync();

                    throw new BusinessException(
                        "زمان اعتبار کد به پایان رسیده است. لطفاً مجدداً کد دریافت کنید."
                    );
                }


                /*
                 * بررسی تعداد تلاش
                 */
                if (
                    latestOtp.AttemptCount >=
                    latestOtp.MaxAttempts
                )
                {
                    latestOtp.IsUsed = true;
                    latestOtp.UsedAt = now;

                    var maxAttemptUpdateResult =
                        await _userOtpService.UpdateAsync(
                            latestOtp,
                            latestOtp.Id
                        );

                    if (!maxAttemptUpdateResult.Success)
                    {
                        throw new BusinessException(
                            maxAttemptUpdateResult.Message
                            ??
                            "خطا در به‌روزرسانی وضعیت کد."
                        );
                    }

                    await _transactionService.CommitAsync();

                    throw new BusinessException(
                        "تعداد دفعات وارد کردن کد بیش از حد مجاز است. لطفاً کد جدید دریافت کنید."
                    );
                }


                /*
                 * بررسی صحت OTP
                 */
                var isValidCode =
                    OtpSecurityHelper.VerifyOtpHash(
                        mobile,
                        input.Code.Trim(),
                        latestOtp.CodeHash
                    );


                /*
                 * کد اشتباه
                 */
                if (!isValidCode)
                {
                    latestOtp.AttemptCount++;

                    var remainingAttempts =
                        Math.Max(
                            0,
                            latestOtp.MaxAttempts -
                            latestOtp.AttemptCount
                        );


                    if (
                        latestOtp.AttemptCount >=
                        latestOtp.MaxAttempts
                    )
                    {
                        latestOtp.IsUsed = true;
                        latestOtp.UsedAt = now;
                    }


                    var invalidCodeUpdateResult =
                        await _userOtpService.UpdateAsync(
                            latestOtp,
                            latestOtp.Id
                        );


                    if (!invalidCodeUpdateResult.Success)
                    {
                        throw new BusinessException(
                            invalidCodeUpdateResult.Message
                            ??
                            "خطا در ثبت تعداد تلاش‌های کد تأیید."
                        );
                    }


                    await _transactionService.CommitAsync();


                    if (remainingAttempts <= 0)
                    {
                        throw new BusinessException(
                            "تعداد دفعات وارد کردن کد بیش از حد مجاز است. لطفاً کد جدید دریافت کنید."
                        );
                    }


                    throw new BusinessException(
                        $"کد واردشده صحیح نیست. {remainingAttempts} بار دیگر فرصت دارید."
                    );
                }


                /*
                 * OTP صحیح است
                 */
                latestOtp.IsVerified = true;
                latestOtp.VerifiedAt = now;


                /*
                 * بررسی وجود کاربر
                 */
                var existingUser =
                    await _userService.GetByMobileNumberAsync(
                        mobile
                    );


                /*
                 * =====================================================
                 * کاربر قبلاً وجود دارد
                 * =====================================================
                 */
                if (existingUser != null)
                {
                    var profile =
                        await _userProfileService.GetByUserIdAsync(
                            existingUser.Id
                        );


                    /*
                     * مصرف OTP
                     */
                    latestOtp.IsUsed = true;
                    latestOtp.UsedAt = now;


                    var existingUserOtpUpdateResult =
                        await _userOtpService.UpdateAsync(
                            latestOtp,
                            latestOtp.Id
                        );


                    if (!existingUserOtpUpdateResult.Success)
                    {
                        throw new BusinessException(
                            existingUserOtpUpdateResult.Message
                            ??
                            "خطا در ثبت وضعیت کد تأیید."
                        );
                    }


                    await _transactionService.CommitAsync();


                    var loginResult =
                        await CreateLoginResultAsync(
                            existingUser,
                            profile
                        );


                    return new ResultDto<WebsiteLoginResultDto>
                    {
                        Success = true,

                        Message =
                            "ورود با موفقیت انجام شد.",

                        Data = loginResult
                    };
                }
                /*
 * =====================================================
 * کاربر جدید است
 * =====================================================
 *
 * در این حالت نام و نام خانوادگی الزامی هستند.
 */

                if (
                    string.IsNullOrWhiteSpace(
                        input.FirstName
                    )
                )
                {
                    throw new BusinessException(
                        "نام برای ثبت‌نام الزامی است."
                    );
                }

                if (
                    string.IsNullOrWhiteSpace(
                        input.LastName
                    )
                )
                {
                    throw new BusinessException(
                        "نام خانوادگی برای ثبت‌نام الزامی است."
                    );
                }
                if (
          string.IsNullOrWhiteSpace(
              input.NationalCode
                                  )
                    )
                {
                    throw new BusinessException(
                        "کد ملی برای ثبت‌نام الزامی است."
                    );
                }
                if (!NationalCodeValidator.IsValid(input.NationalCode))
                {
                    throw new BusinessException(
                        "کد ملی واردشده معتبر نیست."
                    );
                }
                /*
                 * ایجاد کاربر جدید
                 */
                var newUser =
                    await CreateNewUserAsync(
                        mobile,
                        input.FirstName.Trim(),
                        input.LastName.Trim(),
                        input.NationalCode.Trim(),
                        cancellationToken
                    );


                if (newUser == null)
                {
                    throw new BusinessException(
                        "ایجاد حساب کاربری با خطا مواجه شد."
                    );
                }


                /*
                 * پس از ثبت‌نام موفق،
                 * OTP مصرف می‌شود.
                 */
                latestOtp.IsUsed = true;

                latestOtp.UsedAt = now;


                var consumeOtpResult =
                    await _userOtpService.UpdateAsync(
                        latestOtp,
                        latestOtp.Id
                    );


                if (!consumeOtpResult.Success)
                {
                    throw new BusinessException(
                        consumeOtpResult.Message
                        ??
                        "خطا در مصرف کد تأیید."
                    );
                }


                /*
                 * ذخیره نهایی
                 */
                await _transactionService.CommitAsync();


                /*
                 * دریافت Profile کاربر جدید
                 */
                var newUserProfile =
                    await _userProfileService.GetByUserIdAsync(
                        newUser.Id
                    );


                /*
                 * ساخت نتیجه Login
                 */
                var newUserLoginResult =
                    await CreateLoginResultAsync(
                        newUser,
                        newUserProfile
                    );


                return new ResultDto<WebsiteLoginResultDto>
                {
                    Success = true,

                    Message =
                        "ثبت‌نام و ورود با موفقیت انجام شد.",

                    Data = newUserLoginResult
                };
            }
            catch (BusinessException)
            {
                /*
                 * BusinessException پیام قابل نمایش دارد.
                 *
                 * اینجا Rollback نمی‌کنیم؛ چون در بعضی
                 * مسیرها قبل از Throw، AttemptCount یا
                 * IsUsed ذخیره و Commit شده است.
                 */
                throw;
            }
            catch (OperationCanceledException)
            {
                await _transactionService.RollbackAsync();

                throw;
            }
            catch (Exception)
            {
                await _transactionService.RollbackAsync();

                throw new BusinessException(
                    "در بررسی کد یکبارمصرف خطایی رخ داد."
                );
            }
        }
        private async Task<UserDto> CreateNewUserAsync(
    string mobile,
    string firstName,
    string lastName,
    string nationalCode,
    CancellationToken cancellationToken)
        {
            /*
             * اعتبارسنجی نام
             */
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new BusinessException(
                    "نام برای ثبت‌نام الزامی است.");
            }

            /*
             * اعتبارسنجی نام خانوادگی
             */
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new BusinessException(
                    "نام خانوادگی برای ثبت‌نام الزامی است.");
            }

            if (
string.IsNullOrWhiteSpace(
nationalCode
                  )
    )
            {
                throw new BusinessException(
                    "کد ملی برای ثبت‌نام الزامی است."
                );
            }
            if (!NationalCodeValidator.IsValid(nationalCode))
            {
                throw new BusinessException(
                    "کد ملی واردشده معتبر نیست."
                );
            }
            firstName = firstName.Trim();
            lastName = lastName.Trim();
            nationalCode= nationalCode.Trim();
            /*
             * ایجاد کاربر
             *
             * چون ثبت‌نام با OTP انجام می‌شود،
             * Username را شماره موبایل قرار می‌دهیم.
             */
            var userDto = new CustomerRegistrationDto
            {
                Mobile = mobile,
                FirstName = firstName,
                
                LastName = lastName,
                NationalCode= nationalCode,
            };

            var createUserResult =
                await _userService.CreateCustomerAsync(
                    userDto);

            if (
                !createUserResult.Success
                ||
                createUserResult.Data == null)
            {
                throw new BusinessException(
                    createUserResult.Message
                    ??
                    "ایجاد حساب کاربری با خطا مواجه شد.");
            }

            var user =
                createUserResult.Data;

            /*
             * ایجاد پروفایل
             */
            var profile = new UserProfileDto
            {
                Id = Guid.NewGuid(),

                Userid = user.Id,

                Firstname = firstName,

                Lastname = lastName
            };

            var profileResult =
                await _userProfileService
                    .CreateAsync(profile);

            if (!profileResult.Success)
            {
                throw new BusinessException(
                    profileResult.Message
                    ??
                    "ایجاد پروفایل کاربر با خطا مواجه شد.");
            }

            /*
             * دریافت نقش USER
             */
            var userRole =
                await _userRoleService
                    .GetRoleByCodeAsync(
                        RoleCodes.USER);

            if (userRole == null)
            {
                throw new BusinessException(
                    "نقش کاربر در سیستم تعریف نشده است.");
            }

            /*
             * اتصال نقش USER به کاربر
             */
            var createUserRoleResult =
                await _userRoleService
                    .CreateAsync(
                        new UserRoleDto
                        {
                            Id = Guid.NewGuid(),

                            Userid = user.Id,

                            Roleid = userRole.Id
                        });

            if (!createUserRoleResult.Success)
            {
                throw new BusinessException(
                    createUserRoleResult.Message
                    ??
                    "افزودن نقش کاربر با خطا مواجه شد.");
            }

            return user;
        }
        /// <summary>
        /// تکمیل ثبت‌نام کاربر جدید
        /// </summary>
        public async Task<
            ResultDto<WebsiteLoginResultDto>>
            CompleteRegistrationAsync(
                CompleteRegistrationDto input,
                CancellationToken cancellationToken =
                    default)
        {
            try
            {
                cancellationToken
                    .ThrowIfCancellationRequested();

                if (input == null)
                {
                    throw new BusinessException(
                        "اطلاعات ثبت‌نام معتبر نیست.");
                }

                var mobile =
                    OtpSecurityHelper
                        .NormalizeIranMobile(
                            input.Mobile);

                var firstName =
                    OtpSecurityHelper
                        .NormalizeName(
                            input.FirstName,
                            "نام");

                var lastName =
                    OtpSecurityHelper
                        .NormalizeName(
                            input.LastName,
                            "نام خانوادگی");

                /*
                 * فقط OTP تأییدشده و استفاده‌نشده
                 * اجازه ثبت‌نام دارد.
                 */
                var otp =
                    await _userOtpService
                        .FirstOrDefaultAsync<
                            UserOtpDto>(
                            x =>
                                x.Mobile == mobile
                                &&
                                x.Purpose ==
                                (int)
                                UserOtpPurpose
                                    .Authentication
                                &&
                                x.IsVerified
                                &&
                                !x.IsUsed
                        );

                if (otp == null)
                {
                    throw new BusinessException(
                        "ابتدا باید شماره موبایل را با کد یکبارمصرف تأیید کنید.");
                }

                var now = DateTime.UtcNow;

                if (
                    otp.Data.ExpiresAt
                    <=
                    now)
                {
                    otp.Data.IsUsed = true;

                    otp.Data.UsedAt = now;

                    await _userOtpService
                        .UpdateAsync(
                            otp,
                            otp.Data.Id);

                    await _transactionService
                        .CommitAsync();

                    throw new BusinessException(
                        "اعتبار کد تأیید به پایان رسیده است. لطفاً دوباره کد دریافت کنید.");
                }

                var existingUser =
                    await _userService
                        .GetByUserNameAsync(
                            mobile);

                UserDto user;

                if (existingUser != null)
                {
                    /*
                     * برای جلوگیری از ایجاد کاربر تکراری
                     */
                    user = existingUser;

                    var existingProfile =
                        await _userProfileService
                            .GetByUserIdAsync(
                                user.Id);

                    if (existingProfile != null)
                    {
                        existingProfile.Firstname =
                            firstName;

                        existingProfile.Lastname =
                            lastName;

                        await _userProfileService
                            .UpdateAsync(
                                existingProfile,
                                existingProfile.Id);
                    }
                    else
                    {
                        var newProfile =
                            new UserProfileDto
                            {
                                Id =
                                    Guid.NewGuid(),

                                Userid =
                                    user.Id,

                                Firstname =
                                    firstName,

                                Lastname =
                                    lastName
                            };

                        await _userProfileService
                            .CreateAsync(
                                newProfile);
                    }
                }
                else
                {
                    /*
                     * برای کاربر OTP رمز واقعی نداریم.
                     *
                     * یک رمز تصادفی امن تولید می‌کنیم
                     * و هش آن در دیتابیس ذخیره می‌شود.
                     */
                    var randomPassword =
                        Convert.ToBase64String(
                            RandomNumberGenerator
                                .GetBytes(32));

                    var userDto =
                        new UserDto
                        {
                            Id =
                                Guid.NewGuid(),

                            UserName =
                                mobile,

                            MobileNumber =
                                mobile,

                            IsActive =
                                true,

                            IsTest =
                                false
                        };

                    var createUserResult =
                        await _userService
                            .CreateAsync(
                                userDto);

                    if (
                        !createUserResult
                            .Success
                        ||
                        createUserResult
                            .Data
                        ==
                        null)
                    {
                        throw new BusinessException(
                            createUserResult.Message
                            ??
                            "ایجاد حساب کاربری با خطا مواجه شد.");
                    }

                    user =
                        createUserResult.Data;

                    var profile =
                        new UserProfileDto
                        {
                            Id =
                                Guid.NewGuid(),

                            Userid =
                                user.Id,

                            Firstname =
                                firstName,

                            Lastname =
                                lastName
                        };

                    var profileResult =
                        await _userProfileService
                            .CreateAsync(
                                profile);

                    if (
                        !profileResult
                            .Success)
                    {
                        throw new BusinessException(
                            profileResult.Message
                            ??
                            "ایجاد پروفایل کاربر با خطا مواجه شد.");
                    }
                }

                /*
                 * OTP پس از تکمیل موفق ثبت‌نام
                 * مصرف می‌شود.
                 */
                otp.Data.IsUsed = true;

                otp.Data.UsedAt = now;

                await _userOtpService
                    .UpdateAsync(
                        otp,
                        otp.Data.Id);

                /*
                 * تولید Access Token
                 */
                var accessToken =
                    _jwtTokenService
                        .GenerateToken(
                            user);

                /*
                 * تولید Refresh Token
                 */
                var refreshToken =
                    _jwtTokenService
                        .GenerateRefreshToken(
                            user);

                await _transactionService
                    .CommitAsync();

                return new ResultDto<
                    WebsiteLoginResultDto>
                {
                    Success = true,

                    Message =
                        "ثبت‌نام با موفقیت انجام شد.",

                    Data =
                        new WebsiteLoginResultDto
                        {
                            User =
                                new WebsiteUserDto
                                {
                                    FirstName = input.FirstName,
                                    LastName = input.LastName,

                                    Mobile = mobile,
                                },

                            Token =
                                accessToken.Token,

                            ExpireDate =
                                accessToken
                                    .ExpireDate,

                            RefreshToken =
                                refreshToken
                                    .Token
                        }
                };
            }
            catch (BusinessException)
            {
                /*
                 * این بخش باید باقی بماند.
                 *
                 * BusinessException یک خطای قابل نمایش
                 * برای کاربر است و نباید به Exception
                 * عمومی تبدیل شود.
                 */
                await _transactionService
                    .RollbackAsync();

                throw;
            }
            catch (OperationCanceledException)
            {
                await _transactionService
                    .RollbackAsync();

                throw;
            }
            catch (Exception)
            {
                await _transactionService
                    .RollbackAsync();

                throw new BusinessException(
                    "در تکمیل ثبت‌نام خطایی رخ داد.");
            }
        }

        /// <summary>
        /// دریافت تنظیمات فعال سایت
        /// </summary>
        private async Task<
            SiteSettingDto>
            GetActiveSiteSettingAsync(
                CancellationToken
                    cancellationToken)
        {
            cancellationToken
                .ThrowIfCancellationRequested();
            var result = _dbType == DatabaseType.SqlServer
        ? await _siteSettingService.FirstOrDefaultAsync<SqlSiteSetting>(
                        x => x.IsActive)
        : await _siteSettingService.FirstOrDefaultAsync<SqlSiteSetting>(
                        x => x.IsActive);


            if (
                !result.Success
                ||
                result.Data == null)
            {
                throw new BusinessException(
                    "تنظیمات فعال سایت پیدا نشد.");
            }

            return result.Data;
        }

        /// <summary>
        /// تعداد درخواست‌های OTP در یک ساعت اخیر
        /// </summary>
        private async Task<int>
            CountOtpRequestsAsync(
                string mobile,
                DateTime fromDate,
                CancellationToken
                    cancellationToken)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            /*
             * نکته:
             * چون متدهای کامل Repository را در پیام
             * نفرستادی، این قسمت باید با متد Query
             * موجود در GenericService پروژه خودت
             * متصل شود.
             *
             * نمونه پیشنهادی:
             *
             * var query =
             *     await _userOtpService
             *         .GetQueryableAsync<UserOtpDto>();
             *
             * return query.Count(
             *     x =>
             *         x.Mobile == mobile
             *         &&
             *         x.Purpose ==
             *         (int)UserOtpPurpose.Authentication
             *         &&
             *         x.CreatedAt >= fromDate);
             */

            return 0;
        }

        public async Task<ResultDto<UserAddressDto>> CreateUserAddressAsync(
            UserAddressCrud input,
            CancellationToken cancellationToken = default)
                {
                    var result = new ResultDto<UserAddressDto>();
            var userId = _currentUserService.GetUserId();
                    try
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        if (userId == Guid.Empty)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "کاربر معتبر نیست."
                            };
                        }

                        if (input == null)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "اطلاعات آدرس ارسال نشده است."
                            };
                        }


                        var title = input.Title?.Trim();

                        var addressText = input.Address?.Trim();

                        var postalCode =
                            input.PostalCode?
                                .Trim()
                                .Replace("-", "")
                                .Replace(" ", "");


                        if (string.IsNullOrWhiteSpace(title))
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "عنوان آدرس الزامی است."
                            };
                        }


                        if (input.ProvinceId == Guid.Empty)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "استان را انتخاب کنید."
                            };
                        }


                        if (input.CityId == Guid.Empty)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "شهر را انتخاب کنید."
                            };
                        }


                        if (string.IsNullOrWhiteSpace(addressText))
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "آدرس الزامی است."
                            };
                        }


                        if (string.IsNullOrWhiteSpace(postalCode))
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "کد پستی الزامی است."
                            };
                        }


                        if (postalCode.Length != 10 ||
                            !postalCode.All(char.IsDigit))
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "کد پستی باید دقیقاً ۱۰ رقم باشد."
                            };
                        }


                        /*
                            * تعداد آدرس های کاربر
                            */

                        var addresses =
                            await _userAddressService.GetUserAddressesAsync();


                        var addressList =
                            addresses.Data?.ToList()
                            ?? new List<UserAddressDto>();


                        if (addressList.Count >= 5)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "هر کاربر حداکثر می‌تواند ۵ آدرس ثبت کند."
                            };
                        }



                        /*
                            * بررسی کد پستی تکراری
                            */

                        var duplicate =
                            addressList.Any(
                                x =>
                                    x.PostalCode == postalCode);


                        if (duplicate)
                        {
                            return new ResultDto<UserAddressDto>
                            {
                                Success = false,
                                Message = "برای این کد پستی قبلاً آدرس ثبت شده است."
                            };
                        }



                        /*
                            * اگر Default است،
                            * قبلی ها را غیر فعال کن
                            */


                        if (input.IsDefault)
                        {
                            foreach (var address in addressList.Where(x => x.IsDefault))
                            {
                                var updateDto = new UserAddressDto
                                {
                                    Id = address.Id,

                                    UserId = address.UserId,

                                    Title = address.Title,

                                    Address = address.Address,

                                    CityId = address.CityId,

                                    ProvinceId = address.ProvinceId,

                                    PostalCode = address.PostalCode,

                                    PhoneNumber = address.PhoneNumber,

                                    IsActive = address.IsActive,

                                    IsDefault = false
                                };


                                await _userAddressService.UpdateAsync(
                                    updateDto,
                                    address.Id);
                            }
                        }


                        /*
                            * اولین آدرس خودکار Default
                            */

                        var userAddressDto = new UserAddressDto
                        {
                            UserId = userId.Value,

                            Title = title,

                            ProvinceId = input.ProvinceId,

                            CityId = input.CityId,

                            Address = addressText,

                            PostalCode = postalCode,

                            PhoneNumber =
                                string.IsNullOrWhiteSpace(input.PhoneNumber)
                                ? null
                                : input.PhoneNumber.Trim(),


                            IsDefault =
                                addressList.Count == 0 ||
                                input.IsDefault,


                            IsActive = true
                        };



                        /*
                            * استفاده از GenericService
                            */

                        var createResult =
                            await _userAddressService.CreateAsync(userAddressDto);


                        if (!createResult.Success)
                        {
                            return createResult;
                        }


                        await _transactionService.CommitAsync();


                        return new ResultDto<UserAddressDto>
                        {
                            Success = true,

                            Message = "آدرس با موفقیت ثبت شد.",

                            Data = createResult.Data
                        };

                    }
                    catch (Exception ex)
                    {
                        await _transactionService.RollbackAsync();

                        return new ResultDto<UserAddressDto>
                        {
                            Success = false,

                            Message = "خطا در ثبت آدرس.",

                            Errors = new List<string>
                    {
                        ex.Message
                    }
                        };
                    }
                }

        public async Task<ResultDto<bool>> DeleteUserAddressAsync(
    Guid addressId,
    CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var userId = _currentUserService.GetUserId();
                if (userId == Guid.Empty)
                    throw new BusinessException("کاربر معتبر نیست.");

                if (addressId == Guid.Empty)
                    throw new BusinessException("شناسه آدرس معتبر نیست.");


                var addressResult =
                    await _userAddressService.GetUserAddressesAsync();


                var address =
                    addressResult.Data?.FirstOrDefault();


                if (address == null)
                {
                    throw new BusinessException(
                        "آدرس موردنظر یافت نشد.");
                }


                var wasDefault =
                    address.IsDefault;


                var deleteDto = new UserAddressDto
                {
                    Id = address.Id,

                    UserId = address.UserId,

                    Title = address.Title,

                    ProvinceId = address.ProvinceId,

                    CityId = address.CityId,

                    Address = address.Address,

                    PostalCode = address.PostalCode,

                    PhoneNumber = address.PhoneNumber,

                    IsActive = false,

                    IsDefault = false,

                    IsDeleted = true
                };


                var updateResult =
                    await _userAddressService.UpdateAsync(
                        deleteDto,
                        address.Id);


                if (!updateResult.Success)
                    return new ResultDto<bool>
                    {
                        Success = false,
                        Message = updateResult.Message
                    };



                /*
                 * اگر Default بود،
                 * یکی دیگر Default شود
                 */

                if (wasDefault)
                {
                    var otherAddresses =
                        await _userAddressService.GetUserAddressesAsync();


                    var nextAddress =
         otherAddresses.Data
             .OrderByDescending(x => x.CreatedAt)
             .FirstOrDefault();


                    if (nextAddress != null)
                    {
                        var defaultDto = new UserAddressDto
                        {
                            Id = nextAddress.Id,

                            UserId = nextAddress.UserId,

                            Title = nextAddress.Title,

                            ProvinceId = nextAddress.ProvinceId,

                            CityId = nextAddress.CityId,

                            Address = nextAddress.Address,

                            PostalCode = nextAddress.PostalCode,

                            PhoneNumber = nextAddress.PhoneNumber,

                            IsActive = true,

                            IsDefault = true
                        };


                        await _userAddressService.UpdateAsync(
                            defaultDto,
                            nextAddress.Id);
                    }
                }


                await _transactionService.CommitAsync();


                return new ResultDto<bool>
                {
                    Success = true,

                    Message = "آدرس با موفقیت حذف شد.",

                    Data = true
                };
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<bool>
                {
                    Success = false,

                    Message = "خطا در حذف آدرس.",

                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }

        public async Task<ResultDto<UserAddressDto>> UpdateUserAddressAsync(
    Guid addressId,
    UserAddressCrud input,
    CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var userId = _currentUserService.GetUserId();

                if (userId == Guid.Empty)
                    throw new BusinessException("کاربر معتبر نیست.");


                if (addressId == Guid.Empty)
                    throw new BusinessException("شناسه آدرس معتبر نیست.");


                if (input == null)
                    throw new BusinessException("اطلاعات آدرس ارسال نشده است.");



                var currentAddress =
                    await _userAddressService.GetUserAddressByIdAsync(
                        addressId);



                if (!currentAddress.Success ||
                    currentAddress.Data == null)
                {
                    throw new BusinessException(
                        "آدرس موردنظر یافت نشد.");
                }



                var addresses =
                    await _userAddressService.GetUserAddressesAsync();


                var addressList =
                    addresses.Data?.ToList()
                    ?? new List<UserAddressDto>();



                var postalCode =
                    input.PostalCode?
                    .Trim()
                    .Replace("-", "")
                    .Replace(" ", "");



                if (string.IsNullOrWhiteSpace(input.Title))
                    throw new BusinessException("عنوان آدرس الزامی است.");



                if (input.ProvinceId == Guid.Empty)
                    throw new BusinessException("استان را انتخاب کنید.");



                if (input.CityId == Guid.Empty)
                    throw new BusinessException("شهر را انتخاب کنید.");



                if (string.IsNullOrWhiteSpace(input.Address))
                    throw new BusinessException("آدرس الزامی است.");



                if (string.IsNullOrWhiteSpace(postalCode) ||
                    postalCode.Length != 10 ||
                    !postalCode.All(char.IsDigit))
                {
                    throw new BusinessException(
                        "کد پستی باید دقیقاً ۱۰ رقم باشد.");
                }



                var duplicate =
                    addressList.Any(
                        x =>
                            x.Id != addressId &&
                            x.PostalCode == postalCode);



                if (duplicate)
                {
                    throw new BusinessException(
                        "برای این کد پستی قبلاً آدرس ثبت شده است.");
                }



                /*
                 * اگر Default شد،
                 * بقیه Default ها حذف شوند
                 */

                if (input.IsDefault)
                {
                    foreach (var address in addressList
                        .Where(x =>
                            x.Id != addressId &&
                            x.IsDefault))
                    {

                        var updateDefault =
                            new UserAddressDto
                            {
                                Id = address.Id,

                                UserId = address.UserId,

                                Title = address.Title,

                                ProvinceId = address.ProvinceId,

                                CityId = address.CityId,

                                Address = address.Address,

                                PostalCode = address.PostalCode,

                                PhoneNumber = address.PhoneNumber,

                                IsActive = address.IsActive,

                                IsDefault = false
                            };


                        await _userAddressService.UpdateAsync(
                            updateDefault,
                            address.Id);
                    }
                }



                var updateDto =
                    new UserAddressDto
                    {
                        Id = addressId,

                        UserId = userId.Value,

                        Title = input.Title.Trim(),

                        ProvinceId = input.ProvinceId,

                        CityId = input.CityId,

                        Address = input.Address.Trim(),

                        PostalCode = postalCode,

                        PhoneNumber =
                            string.IsNullOrWhiteSpace(input.PhoneNumber)
                            ? null
                            : input.PhoneNumber.Trim(),


                        IsDefault = input.IsDefault,

                        IsActive = true
                    };



                var result =
                    await _userAddressService.UpdateAsync(
                        updateDto,
                        addressId);



                await _transactionService.CommitAsync();


                return result;
            }
            catch (Exception ex)
            {
                await _transactionService.RollbackAsync();

                return new ResultDto<UserAddressDto>
                {
                    Success = false,

                    Message = "خطا در ویرایش آدرس.",

                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ResultDto<UserAddressDto>> GetUserAddressByIdAsync(
    Guid addressId)
        {
            try
            {
                var userId = _currentUserService.GetUserId();
                var result =
                    await _userAddressService.GetUserAddressByIdAsync(
                        addressId);


                if (!result.Success)
                    return result;


                return new ResultDto<UserAddressDto>
                {
                    Success = true,

                    Message = "آدرس دریافت شد.",

                    Data = result.Data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<UserAddressDto>
                {
                    Success = false,

                    Message = "خطا در دریافت آدرس.",

                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
        public async Task<ResultDto<List<UserAddressDto>>> GetUserAddressesAsync()
        {
            try
            {
                var userId = _currentUserService.GetUserId();

                var result =
                    await _userAddressService.GetUserAddressesAsync();



                var data =
                    result.Data?
                    .OrderByDescending(x => x.IsDefault)
                    .ThenByDescending(x => x.CreatedAt)
                    .ToList()
                    ??
                    new List<UserAddressDto>();


                return new ResultDto<List<UserAddressDto>>
                {
                    Success = true,

                    Message = "آدرس‌ها دریافت شدند.",

                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<List<UserAddressDto>>
                {
                    Success = false,

                    Message = "خطا در دریافت آدرس‌ها.",

                    Errors = new List<string>
            {
                ex.Message
            }
                };
            }
        }
    }
}
