using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ActiveDiscountItemDto
    {
        public Guid Id { get; set; }

        public Guid DiscountId { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductVariantId { get; set; }

        public Guid? ProductBrandId { get; set; }

        public Guid? ProductCategoryId { get; set; }

        public int SortOrder { get; set; }
    }
}
