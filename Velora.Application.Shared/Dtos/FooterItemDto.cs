using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class FooterItemDto
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string LinkColor { get; set; }

        public bool? OpenInNewTab { get; set; }

        public bool IsInternalLink { get; set; }

        // Social Icons
        public string Icon { get; set; }

        public string IconAlt { get; set; }

        public string IconColor { get; set; }

        // Trust Logos / Badges
        public string ImageUrl { get; set; }

        public string ImageAlt { get; set; }

        public int Order { get; set; }
    }
}
