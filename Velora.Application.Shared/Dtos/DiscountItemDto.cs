using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Dtos
{
    public class DiscountItemDto
    {
        [Key]
        public Guid Id { get; set; }

        public Guid DiscountId { get; set; }

        public Guid? ProductId { get; set; }

        public Guid? ProductVariantId { get; set; }

        public Guid? ProductCategoryId { get; set; }

        public Guid? ProductBrandId { get; set; }

        public int SortOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

    }
}
