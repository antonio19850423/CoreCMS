using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum SmsProvider
    {
        [Display(Name = "کاوه نگار")]
        Kavenegar=1,
    }
    public enum SmsType
    {
        [Display(Name = "verity")]
        verity=1,
    }
}
