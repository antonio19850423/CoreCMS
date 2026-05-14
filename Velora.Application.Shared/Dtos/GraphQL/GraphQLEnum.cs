using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Dtos.GraphQL
{
    public class GraphQLEnum
    {
        public string Value { get; }
        private GraphQLEnum(string value) => Value = value;

        public static GraphQLEnum Of(string value) => new GraphQLEnum(value);
    }
}
