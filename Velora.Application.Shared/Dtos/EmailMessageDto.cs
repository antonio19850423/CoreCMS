using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class EmailMessageDto
    {
        public string To { get; set; } = default!;

        public string Subject { get; set; } = default!;

        public string Body { get; set; } = default!;

        public bool IsHtml { get; set; } = true;

        public string? FromName { get; set; }
    }
}
