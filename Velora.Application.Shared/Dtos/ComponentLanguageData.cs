using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentLanguageData
    {
        public ComponentLanguageData()
        {
            Items = new List<ComponentItemData>();
        }
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;

        public string Link1Text { get; set; } = string.Empty;
        public string Link1Url { get; set; } = string.Empty;

        public string Link2Text { get; set; } = string.Empty;
        public string Link2Url { get; set; } = string.Empty;

        public string Link3Text { get; set; } = string.Empty;
        public string Link3Url { get; set; } = string.Empty;

        public string Link4Text { get; set; } = string.Empty;
        public string Link4Url { get; set; } = string.Empty;
        public List<ComponentItemData>? Items { get; set; }
    }
}
