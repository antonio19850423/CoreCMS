using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ProductFileSeedModel
    {
        public string FileUrl { get; set; } = null!;

        public string? ThumbnailUrl { get; set; }

        public string? Title { get; set; }

        public string? Alt { get; set; }


        public string MediaType { get; set; } = "Image";


        public bool IsMain { get; set; }

        public int SortOrder { get; set; }
    }
}
