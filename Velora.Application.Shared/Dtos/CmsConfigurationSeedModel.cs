using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class CmsConfigurationSeedModel
    {
        public CmsConfigurationSeedSection Base { get; set; } = new();
        public CmsConfigurationSeedSection Company { get; set; } = new();
        public CmsConfigurationSeedSection Shop { get; set; } = new();
    }
}
