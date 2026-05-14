using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.GraphQL
{
    [AttributeUsage(AttributeTargets.Property)]
    public class GraphQLIgnoreAttribute : Attribute
    {
    }
}
