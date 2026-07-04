using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PageViewDto : PageCrud
    {
        public List<SectionViewDto> Sections { get; set; } = [];
        public List<ContentItemListDto> ContentItems { get; set; } = [];
        public int? TotalCount { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

    }
}
