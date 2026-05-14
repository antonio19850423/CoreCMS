using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos.GraphQL
{
    // Generic paginated connection
    public class GraphQLConnection<T>
    {
        public int TotalCount { get; set; }
        public List<T> Nodes { get; set; } = new();
        public PageInfo PageInfo { get; set; } = new();
    }
}
