using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
{
    public class ContactService : IContactService
    {
        private readonly IEmailService _emailService;
        private readonly ISiteSettingService _siteSettingService;

        public ContactService(
            IEmailService emailService,
            ISiteSettingService siteSettingService)
        {
            _emailService = emailService;
            _siteSettingService = siteSettingService;
        }

        public async Task<ResultDto<ContactUsDto>> SendContactAsync(ContactUsDto input)
        {
            try
            {
                var siteSetting = await _siteSettingService
                    .FirstOrDefaultAsync<SiteSettingDto>(x => x.IsActive);

                if (!siteSetting.Success || siteSetting.Data == null)
                {
                    return new ResultDto<ContactUsDto>
                    {
                        Success = false,
                        Message = "تنظیمات سایت یافت نشد."
                    };
                }

                var setting = siteSetting.Data;

                if (string.IsNullOrWhiteSpace(setting.SmtpHost))
                {
                    return new ResultDto<ContactUsDto>
                    {
                        Success = false,
                        Message = "SMTP Host تنظیم نشده است."
                    };
                }

                if (string.IsNullOrWhiteSpace(setting.SmtpUserName))
                {
                    return new ResultDto<ContactUsDto>
                    {
                        Success = false,
                        Message = "SMTP Username تنظیم نشده است."
                    };
                }

                if (string.IsNullOrWhiteSpace(setting.SmtpPassword))
                {
                    return new ResultDto<ContactUsDto>
                    {
                        Success = false,
                        Message = "SMTP Password تنظیم نشده است."
                    };
                }

                if (string.IsNullOrWhiteSpace(setting.Email))
                {
                    return new ResultDto<ContactUsDto>
                    {
                        Success = false,
                        Message = "ایمیل دریافت‌کننده تنظیم نشده است."
                    };
                }

                var subject = $"پیام جدید فرم تماس - {setting.SiteName}";

                var html = $"""
                <div style="font-family:tahoma;font-size:14px;direction:rtl">

                    <h2 style="color:#16a34a">
                        پیام جدید از فرم تماس
                    </h2>

                    <table style="border-collapse:collapse;width:100%;max-width:600px">

                        <tr>
                            <td style="border:1px solid #ddd;padding:8px;font-weight:bold">
                                نام
                            </td>

                            <td style="border:1px solid #ddd;padding:8px">
                                {input.FirstName}
                            </td>
                        </tr>

                        <tr>
                            <td style="border:1px solid #ddd;padding:8px;font-weight:bold">
                                نام خانوادگی
                            </td>

                            <td style="border:1px solid #ddd;padding:8px">
                                {input.LastName}
                            </td>
                        </tr>

                        <tr>
                            <td style="border:1px solid #ddd;padding:8px;font-weight:bold">
                                ایمیل
                            </td>

                            <td style="border:1px solid #ddd;padding:8px">
                                {input.Email}
                            </td>
                        </tr>

                        <tr>
                            <td style="border:1px solid #ddd;padding:8px;font-weight:bold">
                                پیام
                            </td>

                            <td style="border:1px solid #ddd;padding:8px">
                                {input.Message}
                            </td>
                        </tr>

                    </table>

                    <br/>

                    <small style="color:#999">
                        این پیام به صورت خودکار توسط سیستم ارسال شده است.
                    </small>

                </div>
                """;

                await _emailService.SendAsync(
                    setting.Email,
                    subject,
                    html);

                return new ResultDto<ContactUsDto>
                {
                    Success = true,
                    Message = "پیام شما با موفقیت ارسال شد.",
                    Data = input
                };
            }
            catch (Exception ex)
            {
                return new ResultDto<ContactUsDto>
                {
                    Success = false,
                    Message = "خطا در ارسال ایمیل.",
                    Errors = new List<string>
                    {
                        ex.Message
                    }
                };
            }
        }
    }
}