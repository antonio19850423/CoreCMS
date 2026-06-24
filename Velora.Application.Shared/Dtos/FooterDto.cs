using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class FooterDto
    {
        public string CopyRight { get; set; }
        public List<FooterGroupDto> Groups { get; set; }
    }
}
