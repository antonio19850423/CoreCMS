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
    }
}
