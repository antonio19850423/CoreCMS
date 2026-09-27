using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class SiteMenuViewDto
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string Label { get; set; } = null!;

        public string? Url { get; set; }

        public string? Icon { get; set; }

        public bool OpenInNewTab { get; set; }

        public int SortOrder { get; set; }

        public List<SiteMenuViewDto> Children { get; set; } = new();
    }
}
