using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentRuleView
    {
        public Dictionary<string, bool> Section { get; set; }
        public Dictionary<string, bool> SectionItem { get; set; }
    }
}
