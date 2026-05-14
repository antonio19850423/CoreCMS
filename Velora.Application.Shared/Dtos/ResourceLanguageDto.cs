using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ResourceLanguageDto
    {
        public Guid Id { get; set; }

        public Guid ResourceId { get; set; }

        public string LanguageCode { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
