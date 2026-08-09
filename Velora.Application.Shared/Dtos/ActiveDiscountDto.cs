using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ActiveDiscountDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public byte DiscountType { get; set; }

        public decimal DiscountValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<ActiveDiscountItemDto> Items { get; set; } = new();
    }
}
