using Microsoft.Extensions.Caching.Memory;
using SixLabors.Fonts;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;


namespace Velora.Application.Services
{

    public class CaptchaService
        : ICaptchaService
    {
        private readonly IMemoryCache _cache;

        private const int CaptchaExpirationMinutes = 2;

        private const string CachePrefix =
            "CAPTCHA_";

        public CaptchaService(
            IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<CaptchaGenerateDto>
            GenerateAsync(
                CancellationToken cancellationToken =
                    default)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            var captchaId =
                Guid.NewGuid();

            var captchaCode =
                GenerateCaptchaCode();

            var image =
                GenerateCaptchaImage(
                    captchaCode);

            var cacheKey =
                GetCacheKey(
                    captchaId);

            _cache.Set(
                cacheKey,
                captchaCode,
                new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(
                            CaptchaExpirationMinutes)
                });

            return await Task.FromResult(
                new CaptchaGenerateDto
                {
                    CaptchaId =
                        captchaId,

                    Image =
                        image,

                    ExpirationSeconds =
                        CaptchaExpirationMinutes
                        * 60
                });
        }

        public async Task<
            ResultDto<CaptchaValidationResultDto>>
            ValidateAsync(
                CaptchaValidateDto input,
                CancellationToken cancellationToken =
                    default)
        {
            cancellationToken
                .ThrowIfCancellationRequested();

            if (input == null)
            {
                return new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = false,

                    Message =
                        "اطلاعات کپچا ارسال نشده است."
                };
            }

            if (
                input.CaptchaId ==
                Guid.Empty)
            {
                return new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = false,

                    Message =
                        "شناسه کپچا معتبر نیست."
                };
            }

            if (
                string.IsNullOrWhiteSpace(
                    input.UserInput))
            {
                return new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = false,

                    Message =
                        "کد امنیتی را وارد کنید."
                };
            }

            var cacheKey =
                GetCacheKey(
                    input.CaptchaId);

            if (
                !_cache.TryGetValue(
                    cacheKey,
                    out string? expectedCode)
                ||
                string.IsNullOrWhiteSpace(
                    expectedCode))
            {
                return new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = false,

                    Message =
                        "کد امنیتی منقضی شده یا معتبر نیست.",

                    Data =
                        new CaptchaValidationResultDto
                        {
                            IsValid = false
                        }
                };
            }

            var normalizedExpectedCode =
                NormalizeCaptcha(
                    expectedCode);

            var normalizedUserInput =
                NormalizeCaptcha(
                    input.UserInput);

            var isValid =
                string.Equals(
                    normalizedExpectedCode,
                    normalizedUserInput,
                    StringComparison
                        .OrdinalIgnoreCase);

            /*
             * CAPTCHA باید یک‌بارمصرف باشد.
             *
             * حتی اگر کد اشتباه بود،
             * بهتر است حذف شود تا امکان
             * حدس زدن نامحدود وجود نداشته باشد.
             */
            _cache.Remove(
                cacheKey);

            if (!isValid)
            {
                return new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = false,

                    Message =
                        "کد امنیتی صحیح نیست.",

                    Data =
                        new CaptchaValidationResultDto
                        {
                            IsValid = false
                        }
                };
            }

            return await Task.FromResult(
                new ResultDto<
                    CaptchaValidationResultDto>
                {
                    Success = true,

                    Message =
                        "کد امنیتی با موفقیت تأیید شد.",

                    Data =
                        new CaptchaValidationResultDto
                        {
                            IsValid = true
                        }
                });
        }

        private static string GetCacheKey(
            Guid captchaId)
        {
            return
                $"{CachePrefix}{captchaId}";
        }

        private static string GenerateCaptchaCode()
        {
            const string characters =
                "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

            const int length = 5;

            var result =
                new char[length];

            for (
                var i = 0;
                i < length;
                i++)
            {
                var randomIndex =
                    RandomNumberGenerator
                        .GetInt32(
                            characters.Length);

                result[i] =
                    characters[
                        randomIndex];
            }

            return new string(
                result);
        }

        private static string NormalizeCaptcha(
            string input)
        {
            if (
                string.IsNullOrWhiteSpace(
                    input))
            {
                return string.Empty;
            }

            return input
                .Replace(
                    '۰',
                    '0')
                .Replace(
                    '۱',
                    '1')
                .Replace(
                    '۲',
                    '2')
                .Replace(
                    '۳',
                    '3')
                .Replace(
                    '۴',
                    '4')
                .Replace(
                    '۵',
                    '5')
                .Replace(
                    '۶',
                    '6')
                .Replace(
                    '۷',
                    '7')
                .Replace(
                    '۸',
                    '8')
                .Replace(
                    '۹',
                    '9')
                .Replace(
                    '٠',
                    '0')
                .Replace(
                    '١',
                    '1')
                .Replace(
                    '٢',
                    '2')
                .Replace(
                    '٣',
                    '3')
                .Replace(
                    '٤',
                    '4')
                .Replace(
                    '٥',
                    '5')
                .Replace(
                    '٦',
                    '6')
                .Replace(
                    '٧',
                    '7')
                .Replace(
                    '٨',
                    '8')
                .Replace(
                    '٩',
                    '9')
                .Trim()
                .ToUpperInvariant();
        }

        private static string
            GenerateCaptchaImage(
                string captchaCode)
        {
            const int width = 180;

            const int height = 60;

            var random =
                Random.Shared;

            var svg =
                new StringBuilder();

            svg.AppendLine(
                $"""
        <svg
            xmlns="http://www.w3.org/2000/svg"
            width="{width}"
            height="{height}"
            viewBox="0 0 {width} {height}">
        """
            );

            /*
             * پس‌زمینه
             */
            svg.AppendLine(
                """
        <rect
            width="100%"
            height="100%"
            fill="#F8FAFC"
            rx="8" />
        """
            );

            /*
             * خطوط مزاحم
             */
            for (
                var i = 0;
                i < 7;
                i++)
            {
                var x1 =
                    random.Next(
                        0,
                        width
                    );

                var y1 =
                    random.Next(
                        0,
                        height
                    );

                var x2 =
                    random.Next(
                        0,
                        width
                    );

                var y2 =
                    random.Next(
                        0,
                        height
                    );

                var color =
                    GetRandomCaptchaColor(
                        random
                    );

                svg.AppendLine(
                    $"""
            <line
                x1="{x1}"
                y1="{y1}"
                x2="{x2}"
                y2="{y2}"
                stroke="{color}"
                stroke-width="1"
                opacity="0.35" />
            """
                );
            }

            /*
             * نوشتن کاراکترها
             */
            var x =
                20;

            foreach (
                var character
                in captchaCode)
            {
                var y =
                    random.Next(
                        38,
                        48
                    );

                var rotation =
                    random.Next(
                        -15,
                        16
                    );

                var color =
                    GetRandomCaptchaColor(
                        random
                    );

                svg.AppendLine(
                    $"""
            <text
                x="{x}"
                y="{y}"
                fill="{color}"
                font-family="Arial, sans-serif"
                font-size="32"
                font-weight="bold"
                transform="rotate(
                    {rotation}
                    {x}
                    {y}
                )">
                {character}
            </text>
            """
                );

                x +=
                    28;
            }

            /*
             * نقاط نویز
             */
            for (
                var i = 0;
                i < 100;
                i++)
            {
                var xPoint =
                    random.Next(
                        0,
                        width
                    );

                var yPoint =
                    random.Next(
                        0,
                        height
                    );

                svg.AppendLine(
                    $"""
            <circle
                cx="{xPoint}"
                cy="{yPoint}"
                r="1"
                fill="#94A3B8"
                opacity="0.35" />
            """
                );
            }

            svg.AppendLine(
                "</svg>"
            );

            var svgBytes =
                Encoding.UTF8.GetBytes(
                    svg.ToString()
                );

            var base64 =
                Convert.ToBase64String(
                    svgBytes
                );

            return
                $"data:image/svg+xml;base64,{base64}";
        }
        private static string
    GetRandomCaptchaColor(
        Random random)
        {
            var colors =
                new[]
                {
            "#0F172A",
            "#1E3A8A",
            "#334155",
            "#4C1D95",
            "#7C2D12",
            "#14532D"
                };

            return
                colors[
                    random.Next(
                        colors.Length
                    )
                ];
        }
    }
}
