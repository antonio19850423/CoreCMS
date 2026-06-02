using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos
{
    public class ComponentRuleModel
    {
        public ComponentRules Rules { get; set; } = new ComponentRules();

        public ComponentDefaultData DefaultData { get; set; } = new ComponentDefaultData();
    }
}
