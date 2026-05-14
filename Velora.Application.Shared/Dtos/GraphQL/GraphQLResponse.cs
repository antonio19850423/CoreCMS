using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos.GraphQL
{
    // مدل پاسخ GraphQL
    public class GraphQLResponse<T>
    {
        public T Data { get; set; } = default!;
    }
}
