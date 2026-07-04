using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class GetContentItemsRequest
    {
        public string ContentType { get; set; } // news | article

        public string? CategorySlug { get; set; }

        public string? Search { get; set; }

        public string Sort { get; set; } = "latest"; // latest | oldest

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
