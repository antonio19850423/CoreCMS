using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ContentItemTagMapDto
    {
        public Guid ContentItemId { get; set; }
        public string ContentItemName { get; set; } = default!;
        public List<string> TagCodes { get; set; } = new();
    }

}
