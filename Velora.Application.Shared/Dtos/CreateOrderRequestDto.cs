using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CreateOrderRequestDto
    {
        public string ReceiverFirstName { get; set; } = null!;

        public string ReceiverLastName { get; set; } = null!;

        public string ReceiverNationalCode { get; set; } = null!;

        public string ReceiverPhone { get; set; } = null!;

        public Guid AddressId { get; set; }

        public Guid ShippingMethodId { get; set; }

        public decimal ShippingPrice { get; set; }

        public int PaymentMethod { get; set; }

        public string? Description { get; set; }
    }
}
