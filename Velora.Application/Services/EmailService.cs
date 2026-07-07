using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly ISiteSettingService _siteSettingService;

        public EmailService(ISiteSettingService siteSettingService)
        {
            _siteSettingService = siteSettingService;
        }

        public async Task SendAsync(
            string to,
            string subject,
            string htmlBody,
            string? fromName = null,
            CancellationToken cancellationToken = default)
        {
            var settingResult = await _siteSettingService.FirstOrDefaultAsync<SiteSettingDto>(c => c.IsActive == true);

            if (!settingResult.Success || settingResult.Data == null)
                throw new Exception("Site settings not found.");

            var setting = settingResult.Data;

            if (string.IsNullOrWhiteSpace(setting.SmtpHost))
                throw new Exception("SMTP Host is empty.");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                fromName ?? setting.SiteName,
                setting.SmtpUserName));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();

            await client.ConnectAsync(
            setting.SmtpHost!,
            setting.SmtpPort ?? 587,
            (setting.SmtpEnableSsl ?? true)
                ? SecureSocketOptions.StartTls
                : SecureSocketOptions.None,
            cancellationToken);

            await client.AuthenticateAsync(
                setting.SmtpUserName,
                setting.SmtpPassword,
                cancellationToken);

            await client.SendAsync(message, cancellationToken);

            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}



