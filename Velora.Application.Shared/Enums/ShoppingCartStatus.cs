using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum ShoppingCartStatus
    {
        [Display(Name = "سبد خرید")]
        Cart = 1,

        [Display(Name = "تبدیل شده به سفارش")]
        ConvertedToOrder = 2
    }
}
