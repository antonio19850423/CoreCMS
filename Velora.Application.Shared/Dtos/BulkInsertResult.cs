using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class BulkInsertResult
    {
        /// <summary>
        /// تعداد رکوردهای موفقیت‌آمیز
        /// </summary>
        public int InsertedCount { get; set; }

        /// <summary>
        /// تعداد رکوردهایی که Insert نشدند
        /// </summary>
        public int ErrorCount { get; set; }

        /// <summary>
        /// (اختیاری) لینک فایل خطا یا گزارش
        /// </summary>
        public string? ErrorFileUrl { get; set; }
    }

}
