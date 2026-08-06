using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum PaymentProvider
    {
        [Display(Name = "بانک ملت")]
        Mellat = 1,

        [Display(Name = "بانک ملی ایران")]
        Melli = 2,

        [Display(Name = "بانک سامان")]
        Saman = 3,

        [Display(Name = "بانک پاسارگاد")]
        Pasargad = 4,

        [Display(Name = "بانک پارسیان")]
        Parsian = 5,

        [Display(Name = "بانک تجارت")]
        Tejarat = 6,

        [Display(Name = "بانک صادرات ایران")]
        Saderat = 7,

        [Display(Name = "بانک رفاه کارگران")]
        Refah = 8,

        [Display(Name = "بانک اقتصاد نوین")]
        EghtesadNovin = 9,

        [Display(Name = "بانک آینده")]
        Ayandeh = 10,

        [Display(Name = "بانک دی")]
        Dey = 11,

        [Display(Name = "بانک کشاورزی")]
        Keshavarzi = 12,

        [Display(Name = "بانک سپه")]
        Sepah = 13,


        // پرداخت‌یارها و درگاه‌های اینترنتی

        [Display(Name = "زرین پال")]
        ZarinPal = 20,

        [Display(Name = "آیدی پی")]
        IDPay = 21,

        [Display(Name = "نکست پی")]
        NextPay = 22,

        [Display(Name = "آقای پرداخت")]
        AghayePardakht = 23,

        [Display(Name = "پی دات آی آر")]
        PayIr = 24
    }
}
