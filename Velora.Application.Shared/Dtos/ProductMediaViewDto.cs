using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductMediaViewDto
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string? FileUrl { get; set; }

        public string? ThumbnailUrl { get; set; }

        public string? Title { get; set; }

        public string? Alt { get; set; }

        public string? MediaType { get; set; }

        public bool IsMain { get; set; }

        public int SortOrder { get; set; }
    }
}
