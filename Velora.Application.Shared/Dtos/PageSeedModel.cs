using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class PageSeedModel
    {
        public string PageName { get; set; }
        public string Slug { get; set; }
        public bool IsPublished { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public List<PageComponentSeedDto> Components { get; set; }
    }
}
