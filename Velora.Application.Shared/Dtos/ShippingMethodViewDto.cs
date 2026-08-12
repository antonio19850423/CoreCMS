using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ShippingMethodViewDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int EstimatedDays { get; set; }

        public bool IsNationwide { get; set; }

        public bool IsDefault { get; set; }

        public List<ShippingMethodCityViewDto> Cities { get; set; } = new();
    }
}
