using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CreateProductReviewDto
    {
        public Guid ProductId { get; set; }

        public string? Title { get; set; }

        public int Rate { get; set; }

        public string Comment { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string PersianDate { get; set; } = null!;
    }
}
