using Kavenegar;
using Kavenegar;
using Kavenegar.Exceptions;
using Kavenegar.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{




    public class KavenegarSmsService : ISmsService
    {
        private readonly ISmsSettingService _smsSettingService;

        private readonly ISiteSettingService _siteSettingService;

        private readonly ISmsLogService _smsLogService;
        private readonly DatabaseType _dbType;
        private readonly IWebHostEnvironment _env;
        public KavenegarSmsService(
            ISmsSettingService smsSettingService,
            ISiteSettingService siteSettingService,
            ISmsLogService smsLogService, IConfiguration configuration, IWebHostEnvironment env)
        {
            _smsSettingService = smsSettingService;

            _siteSettingService = siteSettingService;

            _smsLogService = smsLogService;
            var dbTypeString = configuration.GetValue<string>("Database:Provider") ?? "PostgreSql";
            _dbType = dbTypeString.Equals("SqlServer", StringComparison.OrdinalIgnoreCase)
                ? DatabaseType.SqlServer
                : DatabaseType.PostgreSql;
            _env = env;
        }

        public async Task SendOtpAsync(
            string mobile,
            string code,
            int expirationMinutes,
            CancellationToken cancellationToken = default)
        {

            var smsSettingResult = _dbType == DatabaseType.SqlServer
? await _smsSettingService.FirstOrDefaultAsync<
            SqlSmsSetting>(
                        x => x.IsActive)
: await _smsSettingService.FirstOrDefaultAsync<
            SqlSmsSetting>(
                        x => x.IsActive);


            if (!smsSettingResult.Success ||
                smsSettingResult.Data == null)
            {
                throw new BusinessException(
                    "تنظیمات سرویس پیامک پیدا نشد.");
            }

            var smsSetting = smsSettingResult.Data;

            if (string.IsNullOrWhiteSpace(
                smsSetting.ApiKey))
            {
                throw new BusinessException(
                    "کلید API سرویس پیامک وارد نشده است.");
            }
            var siteSettingResult = _dbType == DatabaseType.SqlServer
       ? await _siteSettingService.FirstOrDefaultAsync<SqlSiteSetting>(
                       x => x.IsActive)
       : await _siteSettingService.FirstOrDefaultAsync<SqlSiteSetting>(
                       x => x.IsActive);

            if (!siteSettingResult.Success ||
                siteSettingResult.Data == null)
            {
                throw new BusinessException(
                    "تنظیمات سایت پیدا نشد.");
            }

            var siteSetting = siteSettingResult.Data;

            try
            {
                var api = new KavenegarApi(
                    smsSetting.ApiKey);

                var result = api.VerifyLookup(
                    receptor: mobile,
                    token: code,
                    token2: siteSetting.SiteName,
                    token3: expirationMinutes.ToString(),
                    template: SmsTemplateNames.AUTHOTP);

                if (result == null)
                {
                    throw new BusinessException(
                        "پاسخی از سرویس پیامک دریافت نشد.");
                }

                await _smsLogService
                    .CreateAsync(
                        new SmsLogCrud
                        {
                            Mobile = mobile,

                            SmsType =
                                SmsTemplateNames.AUTHOTP,

                            Provider =
                                (int)SmsProvider.Kavenegar,

                            ProviderMessageId =
                                result.Messageid
                                    .ToString(),

                            Message =
                                result.Message,

                            IsSuccess = true
                        });
            }
            catch (ApiException ex)
            {
                await _smsLogService
                    .CreateAsync(
                        new SmsLogCrud
                        {
                            Mobile = mobile,

                            SmsType =
                                SmsTemplateNames.AUTHOTP,

                            Provider =
                                (int)SmsProvider.Kavenegar,

                            ErrorMessage =
                                ex.Message,
                            Message = ex.Message,

                            IsSuccess = false
                        });
                // فقط برای محیط Development و تست موقت
                if (_env.IsDevelopment()
                    && ex.Message.Contains(
                        "احراز هویت",
                        StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
                throw new BusinessException(
                    $"خطا در ارسال پیامک: {ex.Message}",
                    ex);
            }
            catch (HttpException ex)
            {
                await _smsLogService
                    .CreateAsync(
                        new SmsLogCrud
                        {
                            Mobile = mobile,

                            SmsType =
                                SmsTemplateNames.AUTHOTP,

                            Provider =
                                (int)SmsProvider.Kavenegar,

                            ErrorMessage =
                                ex.Message,

                            IsSuccess = false
                        });

                throw new BusinessException(
                    "ارتباط با سرویس پیامک برقرار نشد.",
                    ex);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                await _smsLogService
                    .CreateAsync(
                        new SmsLogCrud
                        {
                            Mobile = mobile,

                            SmsType =
                                SmsTemplateNames.AUTHOTP,

                            Provider =
                                (int)SmsProvider.Kavenegar,

                            ErrorMessage =
                                ex.Message,
                            Message= ex.Message,

                            IsSuccess = false
                        });

                throw new BusinessException(
                    "خطای غیرمنتظره‌ای هنگام ارسال پیامک رخ داد.",
                    ex);
            }
        }

        public async Task<decimal?> GetBalanceAsync(
            CancellationToken cancellationToken = default)
        {
            var smsSettingResult =
                await _smsSettingService
                    .FirstOrDefaultAsync<SmsSettingDto>(
                        x => x.IsActive);

            if (!smsSettingResult.Success ||
                smsSettingResult.Data == null)
            {
                throw new BusinessException(
                    "تنظیمات سرویس پیامک پیدا نشد.");
            }

            var smsSetting =
                smsSettingResult.Data;

            if (string.IsNullOrWhiteSpace(
                smsSetting.ApiKey))
            {
                throw new BusinessException(
                    "کلید API سرویس پیامک وارد نشده است.");
            }

            try
            {
                var api = new KavenegarApi(
                    smsSetting.ApiKey);

                var result = api.AccountInfo();

                if (result == null)
                {
                    throw new BusinessException(
                        "پاسخی از سرویس کاوه‌نگار دریافت نشد.");
                }

                return result.RemainCredit;
            }
            catch (ApiException ex)
            {
                throw new BusinessException(
                    $"خطا در دریافت موجودی پیامک: {ex.Message}",
                    ex);
            }
            catch (HttpException ex)
            {
                throw new BusinessException(
                    "ارتباط با سرویس کاوه‌نگار برقرار نشد.",
                    ex);
            }
            // مهم:
            // BusinessException قبلاً در همین لایه با پیام مناسب ایجاد شده است.
            // بنابراین نباید دوباره توسط catch (Exception) گرفته و به یک
            // BusinessException جدید تبدیل شود؛ چون پیام اصلی از بین می‌رود.
            // این بخش Exception را بدون تغییر به لایه بالاتر ارسال می‌کند.
            catch (BusinessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BusinessException(
                    "خطای غیرمنتظره‌ای هنگام دریافت موجودی پیامک رخ داد.",
                    ex);
            }
        }
    }
}
