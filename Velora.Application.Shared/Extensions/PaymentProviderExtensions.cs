using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Enums;

namespace Velora.Application.Shared.Extensions
{
    public static class PaymentProviderExtensions
    {
        public static string GetCode(this PaymentProvider provider)
        {
            return provider switch
            {
                PaymentProvider.Mellat => "MELLAT",

                PaymentProvider.Melli => "MELLI",

                PaymentProvider.Saman => "SAMAN",

                PaymentProvider.Pasargad => "PASARGAD",

                PaymentProvider.Parsian => "PARSIAN",

                PaymentProvider.Tejarat => "TEJARAT",

                PaymentProvider.Saderat => "SADERAT",

                PaymentProvider.Refah => "REFAH",

                PaymentProvider.EghtesadNovin => "EGHTESADNOVIN",

                PaymentProvider.Ayandeh => "AYANDEH",

                PaymentProvider.Dey => "DEY",

                PaymentProvider.Keshavarzi => "KESHAVARZI",

                PaymentProvider.Sepah => "SEPAH",


                // Payment Gateways

                PaymentProvider.ZarinPal => "ZARINPAL",

                PaymentProvider.IDPay => "IDPAY",

                PaymentProvider.NextPay => "NEXTPAY",

                PaymentProvider.AghayePardakht => "AGHAYEPARDOKHT",

                PaymentProvider.PayIr => "PAYIR",


                _ => string.Empty
            };
        }
    }
}
