
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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

        public UploadService(IWebHostEnvironment env,IConfiguration config)
            {
            _env = env;
            _config = config;
            }

        public async Task<ResultDto<UploadResultDto?>> UploadImageAsync(IFormFile file,string name)
            {
            var result = new ResultDto<UploadResultDto?>();

            // 1️⃣ بررسی ورودی
            if(file == null || file.Length == 0)
                return new ResultDto<UploadResultDto?>
                    {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "فایلی ارسال نشده است."
                    };

            const long maxFileSize = 1 * 1024 * 1024; // 1 MB
            if(file.Length > maxFileSize)
                return new ResultDto<UploadResultDto?>
                    {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = $"حجم فایل نباید بیشتر از {maxFileSize / (1024 * 1024)} MB باشد."
                    };

            // 2️⃣ بررسی فرمت فایل
            var allowedExtensions = new[] { ".jpg",".jpeg",".png",".gif",".webp" };
            var ext = System.IO.Path.GetExtension(file.FileName)?.ToLowerInvariant();
            if(ext == null || !allowedExtensions.Contains(ext))
                return new ResultDto<UploadResultDto?>
                    {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Success = false,
                    Message = "فقط فرمت‌های تصویری مجاز هستند."
                    };

            try
                {
                // 3️⃣ مسیر ذخیره فایل
                var uploadsRoot = System.IO.Path.Combine(
                    _env.WebRootPath ?? System.IO.Path.Combine(Directory.GetCurrentDirectory(),"wwwroot"),
                    "uploads",
                    "images"
                );
                if(!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                // 4️⃣ ساخت نام امن و یکتا
                var safeName = string.IsNullOrWhiteSpace(name) ? "file" : name.Trim().Replace(" ","-").ToLowerInvariant();
                var shortGuid = Guid.NewGuid().ToString("N").Substring(0,8); // 8 کاراکتر اول GUID
                var uniqueName = $"{safeName}_{shortGuid}{ext}";
                var filePath = System.IO.Path.Combine(uploadsRoot,uniqueName);

                // 5️⃣ ذخیره فیزیکی فایل
                await using var stream = new FileStream(filePath,FileMode.Create);
                await file.CopyToAsync(stream);

                // 6️⃣ ساخت URL دسترسی
                var fileUrl = $"/uploads/images/{uniqueName}";

                // 7️⃣ آماده‌سازی نتیجه
                result.Success = true;
                result.StatusCode = StatusCodes.Status200OK;
                result.Message = "آپلود موفق بود";
                result.Data = new UploadResultDto
                    {
                    Success = true,
                    Url = fileUrl,
                    FileName = uniqueName,
                    Message = "آپلود موفق بود"
                    };

                return result;
                }
            catch(Exception ex)
                {
                // 8️⃣ خطا
                return new ResultDto<UploadResultDto?>
                    {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Success = false,
                    Message = ex.Message
                    };
                }
            }

        }
    }
