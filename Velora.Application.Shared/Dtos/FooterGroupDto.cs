using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class FooterGroupDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public int Order { get; set; }
        public string Icon { get; set; } 
        public string IconColor { get; set; }
        public string IconAlt { get; set; }
        public List<FooterItemDto> Items { get; set; }
    }
}
