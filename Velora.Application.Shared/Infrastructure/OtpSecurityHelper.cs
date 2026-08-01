using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;


namespace Velora.Application.Shared.Infrastructure
{

    public static class OtpSecurityHelper
    {
        /// <summary>
        /// شماره موبایل را به فرمت استاندارد ایران تبدیل می‌کند.
        /// مثال:
        /// 09121234567 => 09121234567
        /// 989121234567 => 09121234567
        /// +989121234567 => 09121234567
        /// </summary>
        public static string NormalizeIranMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                throw new BusinessException("شماره موبایل وارد نشده است.");

            var normalized = mobile
                .Trim()
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty)
                .Replace("(", string.Empty)
                .Replace(")", string.Empty);

            if (normalized.StartsWith("+98"))
            {
                normalized = "0" + normalized[3..];
            }
            else if (normalized.StartsWith("0098"))
            {
                normalized = "0" + normalized[4..];
            }
            else if (normalized.StartsWith("98") &&
                     normalized.Length == 12)
            {
                normalized = "0" + normalized[2..];
            }
            else if (normalized.StartsWith("9") &&
                     normalized.Length == 10)
            {
                normalized = "0" + normalized;
            }

            if (!IsValidIranMobile(normalized))
            {
                throw new BusinessException(
                    "شماره موبایل واردشده معتبر نیست.");
            }

            return normalized;
        }

        /// <summary>
        /// بررسی معتبر بودن شماره موبایل ایران
        /// </summary>
        public static bool IsValidIranMobile(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                return false;

            return System.Text.RegularExpressions.Regex.IsMatch(
                mobile,
                @"^09\d{9}$");
        }

        /// <summary>
        /// تولید کد OTP با استفاده از RandomNumberGenerator
        /// </summary>
        public static string GenerateOtpCode(int length)
        {
            if (length < 4 || length > 10)
            {
                throw new BusinessException(
                    "طول کد یکبارمصرف باید بین ۴ تا ۱۰ رقم باشد.");
            }

            var minimum = (int)Math.Pow(
                10,
                length - 1);

            var maximum = (int)Math.Pow(
                10,
                length);

            var code = RandomNumberGenerator.GetInt32(
                minimum,
                maximum);

            return code.ToString();
        }

        /// <summary>
        /// هش کردن کد OTP
        /// </summary>
        public static string HashOtp(
            string mobile,
            string code)
        {
            if (string.IsNullOrWhiteSpace(mobile))
                throw new ArgumentException(
                    "شماره موبایل خالی است.",
                    nameof(mobile));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException(
                    "کد یکبارمصرف خالی است.",
                    nameof(code));

            var value = $"{mobile}:{code}";

            var bytes = Encoding.UTF8.GetBytes(value);

            var hash = SHA256.HashData(bytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// مقایسه امن هش OTP
        /// </summary>
        public static bool VerifyOtpHash(
            string mobile,
            string code,
            string storedHash)
        {
            if (string.IsNullOrWhiteSpace(storedHash))
                return false;

            var calculatedHash = HashOtp(
                mobile,
                code);

            var calculatedBytes =
                Convert.FromBase64String(
                    calculatedHash);

            var storedBytes =
                Convert.FromBase64String(
                    storedHash);

            return CryptographicOperations
                .FixedTimeEquals(
                    calculatedBytes,
                    storedBytes);
        }

        /// <summary>
        /// بررسی نام
        /// </summary>
        public static string NormalizeName(
            string? value,
            string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BusinessException(
                    $"{fieldName} وارد نشده است.");
            }

            var result = value.Trim();

            if (result.Length < 2)
            {
                throw new BusinessException(
                    $"{fieldName} باید حداقل ۲ کاراکتر باشد.");
            }

            if (result.Length > 100)
            {
                throw new BusinessException(
                    $"{fieldName} نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");
            }

            return result;
        }
    }
}
