using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "در انتظار پرداخت")]
        Pending = 1,

        [Display(Name = "پرداخت موفق")]
        Paid = 2,

        [Display(Name = "پرداخت ناموفق")]
        Failed = 3,

        [Display(Name = "لغو شده")]
        Cancelled = 4
    }
}
