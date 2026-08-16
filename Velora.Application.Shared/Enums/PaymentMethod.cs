using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum PaymentMethod
    {
        [Display(Name = "پرداخت آنلاین")]
        Online = 1,

        [Display(Name = "کارت به کارت")]
        CardToCard = 2
    }
}
