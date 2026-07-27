using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class UpdateCartItemDto
    {
        public Guid CartItemId { get; set; }

        public int Quantity { get; set; }
    }
}
