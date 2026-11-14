using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PageComponentSeedDto : SectionCrud
    {
        public PageComponentSeedDto()
        {
            Items = new List<SectionItemCrud>();
        }
        public string Code { get; set; } = default!;
        public string Type { get; set; } = default!;

        public List<SectionItemCrud> Items { get; set; } = [];
    }
}
