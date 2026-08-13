using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ApplyCouponRequestDto
    {
        public Guid ShoppingCartId { get; set; }

        public string CouponCode { get; set; } = string.Empty;
    }
}
