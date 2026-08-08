using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum DiscountType
    {
        [Display(Name = "درصدی")]
        Increase = 1,

        [Display(Name = "مبلغ ثابت")]
        Decrease = 2
    }
    public enum CouponType
    {
        [Display(Name = "درصدی")]
        Increase = 1,

        [Display(Name = "مبلغ ثابت")]
        Decrease = 2
    }
}
