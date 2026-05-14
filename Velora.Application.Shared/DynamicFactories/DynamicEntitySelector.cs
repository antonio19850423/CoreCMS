using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.DynamicFactories
{
    public static class DynamicEntitySelector
    {
        public static Type Select<TSql, TPg>(IConfiguration config)
        {
            var provider = config["DatabaseProvider"]; // "SqlServer" یا "PostgreSQL"
            return provider == "PostgreSQL" ? typeof(TPg) : typeof(TSql);
        }
    }
}
