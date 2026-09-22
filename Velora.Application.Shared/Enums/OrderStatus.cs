using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "ثبت شده")]
        Registered = 1,

        [Display(Name = "در حال پردازش")]
        Processing = 2,

        [Display(Name = "آماده ارسال")]
        Preparing = 3,

        [Display(Name = "ارسال شده")]
        Shipped = 4,

        [Display(Name = "تحویل شده")]
        Delivered = 5,

        [Display(Name = "لغو شده")]
        Cancelled = 6
    }
}
