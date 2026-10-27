using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SitemapUrlDto
    {
        public string Url { get; set; } = string.Empty;

        public DateTime? LastModified { get; set; }
    }
}
