using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentDefaultData
    {
        public ComponentLanguageData Rtl { get; set; } = new ComponentLanguageData();
        public ComponentLanguageData Ltr { get; set; } = new ComponentLanguageData();
    }
}
