using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentItemData
    {
        public Guid Id { get; set; }

        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        // core content
        public string? Title { get; set; }
        public string? Subtitle { get; set; }
        public string? Description { get; set; }

        // media
        public string? ImageUrl { get; set; }
        public string? ImageAlt { get; set; }

        public string? Icon { get; set; }
        public string? IconAlt { get; set; }

        // optional feature-only fields (NOT always used)
        public string? Price { get; set; }

        // links
        public string? Link1Text { get; set; }
        public string? Link1Url { get; set; }

        public string? Link2Text { get; set; }
        public string? Link2Url { get; set; }

        public string? Link3Text { get; set; }
        public string? Link3Url { get; set; }

        public string? Link4Text { get; set; }
        public string? Link4Url { get; set; }

        // colors
        public string? BackgroundColor { get; set; }
        public string? TitleColor { get; set; }
        public string? SubtitleColor { get; set; }
        public string? DescriptionColor { get; set; }

        public string? IconColor { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Features { get; set; }
    }
}
