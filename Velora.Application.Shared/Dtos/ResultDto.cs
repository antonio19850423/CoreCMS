using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ResultDto<T>
    {
        public ResultDto()
            {
                    Errors = new List<string>();
            }
        public bool Success { get; set; }           // آیا عملیات موفق بود
        public string Message { get; set; }         // پیام برای کاربر یا لاگ
        public T Data { get; set; }                 // داده اصلی (می‌تونه توکن، کاربر، لیست، هر چیزی باشه)
        public List<string> Errors { get; set; }    // لیست خطاها (اختیاری)
        public object Meta { get; set; }            // متادیتا (مثلاً ExpireDate توکن و ...)
        public int StatusCode { get; set; }
    }

}
