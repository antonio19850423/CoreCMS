
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

namespace Velora.Application.Services
    {
    public class UploadService:IUploadService
        {
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly ILogger<UploadService> _logger;
        public UploadService(IWebHostEnvironment env,IConfiguration config, ILogger<UploadService> logger)
            {
            _env = env;
            _config = config;
            _logger = logger;
            }



        public async Task<ResultDto<UploadResultDto?>> UploadImageAsync(
            IFormFile file,
            string name)
        {
            // 1. بررسی ورودی
            if (file == null || file.Length == 0)
            {
                return new ResultDto<UploadResultDto?>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "فایلی ارسال نشده است."
                };
            }

            // 2. بررسی حجم فایل
            const long maxFileSize = 1 * 1024 * 1024; // 1 MB

            if (file.Length > maxFileSize)
            {
                return new ResultDto<UploadResultDto?>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "حجم فایل نباید بیشتر از 1 MB باشد."
                };
            }

            // 3. بررسی فرمت فایل
            var allowedExtensions = new[]
            {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

            var ext = System.IO.Path.GetExtension(file.FileName)
                ?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(ext) ||
                !allowedExtensions.Contains(ext))
            {
                return new ResultDto<UploadResultDto?>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "فقط فرمت‌های تصویری مجاز هستند."
                };
            }

            try
            {
                // 4. خواندن تنظیمات FileStorage
                var basePath = _config["FileStorage:BasePath"]
                    ?? "wwwroot/uploads";

                var defaultFolder = _config["FileStorage:DefaultFolder"]
                    ?? "images";

                var useDomainFolder = _config
                    .GetValue<bool>("FileStorage:UseDomainFolder");

                var domainFolder = _config["FileStorage:DomainFolder"];

                // 5. تعیین مسیر اصلی wwwroot
                var webRoot = _env.WebRootPath;

                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = System.IO.Path.Combine(
                        _env.ContentRootPath,
                        "wwwroot"
                    );
                }

                webRoot = System.IO.Path.GetFullPath(webRoot);

                // 6. تعیین مسیر پایه ذخیره‌سازی با مسیر کامل
                string uploadsBasePath;

                if (System.IO.Path.IsPathRooted(basePath))
                {
                    uploadsBasePath =
                        System.IO.Path.GetFullPath(basePath);
                }
                else
                {
                    uploadsBasePath =
                        System.IO.Path.GetFullPath(
                            System.IO.Path.Combine(
                                _env.ContentRootPath,
                                basePath
                            )
                        );
                }

                // 7. انتخاب پوشه اختصاصی یا پیش‌فرض
                var selectedFolder = defaultFolder;

                if (useDomainFolder &&
                    !string.IsNullOrWhiteSpace(domainFolder))
                {
                    var safeDomainFolder = new string(
                        domainFolder.Trim()
                            .Where(c =>
                                char.IsLetterOrDigit(c) ||
                                c == '-' ||
                                c == '_')
                            .ToArray()
                    );

                    if (!string.IsNullOrWhiteSpace(safeDomainFolder))
                    {
                        selectedFolder = safeDomainFolder;
                    }
                }

                // 8. اعتبارسنجی نام پوشه انتخاب‌شده
                var safeSelectedFolder = new string(
                    selectedFolder.Trim()
                        .Where(c =>
                            char.IsLetterOrDigit(c) ||
                            c == '-' ||
                            c == '_')
                        .ToArray()
                );

                if (string.IsNullOrWhiteSpace(safeSelectedFolder))
                {
                    throw new InvalidOperationException(
                        "نام پوشه ذخیره‌سازی در تنظیمات FileStorage معتبر نیست."
                    );
                }

                // 9. ساخت مسیر نهایی پوشه ذخیره‌سازی
                var uploadsRoot =
                    System.IO.Path.GetFullPath(
                        System.IO.Path.Combine(
                            uploadsBasePath,
                            safeSelectedFolder
                        )
                    );

                // جلوگیری از خروج مسیر ذخیره‌سازی از پوشه پایه
                var normalizedBase =
                    System.IO.Path.GetFullPath(uploadsBasePath)
                        .TrimEnd(
                            System.IO.Path.DirectorySeparatorChar,
                            System.IO.Path.AltDirectorySeparatorChar
                        )
                    + System.IO.Path.DirectorySeparatorChar;

                if (!uploadsRoot.StartsWith(
                    normalizedBase,
                    StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException(
                        "مسیر ذخیره‌سازی فایل معتبر نیست."
                    );
                }

                // 10. ایجاد خودکار پوشه انتخاب‌شده در صورت نبود
                Directory.CreateDirectory(uploadsRoot);

                // 11. ساخت نام فایل امن و یکتا
                var safeName = string.IsNullOrWhiteSpace(name)
                    ? "file"
                    : name.Trim().ToLowerInvariant();

                safeName = safeName.Replace(" ", "-");

                safeName = new string(
                    safeName
                        .Where(c =>
                            char.IsLetterOrDigit(c) ||
                            c == '-' ||
                            c == '_')
                        .ToArray()
                );

                if (string.IsNullOrWhiteSpace(safeName))
                {
                    safeName = "file";
                }

                var shortGuid = Guid.NewGuid()
                    .ToString("N")[..8];

                var uniqueName = $"{safeName}_{shortGuid}{ext}";

                var filePath = System.IO.Path.Combine(
                    uploadsRoot,
                    uniqueName
                );

                // 12. ذخیره فیزیکی فایل
                await using (var stream = new FileStream(
                    filePath,
                    FileMode.CreateNew,
                    FileAccess.Write,
                    FileShare.None))
                {
                    await file.CopyToAsync(stream);
                }

                // 13. بررسی قرار داشتن فایل داخل wwwroot
                var webRootPrefix = webRoot.TrimEnd(
                    System.IO.Path.DirectorySeparatorChar,
                    System.IO.Path.AltDirectorySeparatorChar
                ) + System.IO.Path.DirectorySeparatorChar;

                if (!filePath.StartsWith(
                    webRootPrefix,
                    StringComparison.OrdinalIgnoreCase))
                {
                    // حذف فایل در صورت ذخیره شدن خارج از wwwroot
                    System.IO.File.Delete(filePath);

                    throw new InvalidOperationException(
                        "مسیر ذخیره‌سازی باید داخل پوشه wwwroot باشد."
                    );
                }

                // 14. ساخت URL دسترسی
                var relativePath =
                    System.IO.Path.GetRelativePath(
                        webRoot,
                        filePath
                    );

                var fileUrl = "/" + relativePath
                    .Replace('\\', '/');

                // 15. بازگرداندن نتیجه
                return new ResultDto<UploadResultDto?>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "آپلود موفق بود",
                    Data = new UploadResultDto
                    {
                        Success = true,
                        Url = fileUrl,
                        FileName = uniqueName,
                        Message = "آپلود موفق بود"
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "خطا هنگام آپلود تصویر {FileName}",
                    file?.FileName
                );

                return new ResultDto<UploadResultDto?>
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = "هنگام ذخیره تصویر خطایی رخ داد."
                };
            }
        }




        public async Task<ResultDto<bool>> DeleteImageAsync(string url)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(url))
                {
                    return new ResultDto<bool>
                    {
                        Success = false,
                        StatusCode = 400,
                        Message = "آدرس فایل ارسال نشده است."
                    };
                }


                var fileName = System.IO.Path.GetFileName(url);


                var uploadsRoot = System.IO.Path.Combine(
                    _env.WebRootPath ?? System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                    "uploads",
                    "images"
                );


                var filePath = System.IO.Path.Combine(
                    uploadsRoot,
                    fileName
                );


                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }


                return new ResultDto<bool>
                {
                    Success = true,
                    StatusCode = 200,
                    Data = true,
                    Message = "فایل حذف شد."
                };

            }
            catch (Exception ex)
            {
                return new ResultDto<bool>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }
        public async Task<ResultDto<UploadResultDto?>> ReplaceImageAsync(
    IFormFile file,
    string name,
    string? oldUrl)
        {

            // اول فایل قبلی را حذف کن
            if (!string.IsNullOrWhiteSpace(oldUrl))
            {
                await DeleteImageAsync(oldUrl);
            }


            // بعد فایل جدید را آپلود کن
            return await UploadImageAsync(file, name);

        }

    }
    }
