using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class LocalizationkeyDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;

        public string Type { get; set; } = null!;
        public int? Order { get; set; } = null!;

        public bool IsTest { get; set; }
    }
}
