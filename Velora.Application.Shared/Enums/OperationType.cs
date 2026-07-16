using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Enums
{
    public enum OperationType
    {
        [Display(Name = "افزایش")]
        Increase = 1,

        [Display(Name = "کاهش")]
        Decrease = 2
    }
}
