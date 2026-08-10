using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface ICookieService:IBaseService
    {
        /// <summary>
        /// دریافت مقدار کوکی
        /// </summary>
        string? Get(string key);


        /// <summary>
        /// ایجاد یا بروزرسانی کوکی
        /// </summary>
        void Set(
            string key,
            string value,
            int expireDays = 30);



        /// <summary>
        /// حذف کوکی
        /// </summary>
        void Remove(string key);



        /// <summary>
        /// بررسی وجود کوکی
        /// </summary>
        bool Exists(string key);



        /// <summary>
        /// دریافت یا ایجاد مقدار جدید
        /// </summary>
        string GetOrCreate(
            string key,
            Func<string> generator,
            int expireDays = 30);

         void Restore(
    string key,
    string value,
    int expireDays = 30);
    }
}
