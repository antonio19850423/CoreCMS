using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PageComponentSeedDto
    {
        public string Code { get; set; } = default!;
        public string Type { get; set; } = default!;
        public string? TextColor { get; set; }
        public string? ButtonColor { get; set; }
        public string? BackgroundColor { get; set; }

        public string? BackgroundGradient { get; set; }
        public string? HeaderColor { get; set; }
        public string? SubtitleColor { get; set; }
        public string? DescriptionColor { get; set; }
        public string? Link1Color { get; set; }
        public string? Link2Color { get; set; }
        public string? Link3Color { get; set; }
        public string? Link4Color { get; set; }
        public string? IconColor { get; set; }
        

    }
}
