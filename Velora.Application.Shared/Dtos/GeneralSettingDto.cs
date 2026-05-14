using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class GeneralSettingDto
    {
        public Guid Id { get; set; }

        public string Key { get; set; } = null!;

        public string Value { get; set; } = null!;

        public string? Description { get; set; }
    }
}
