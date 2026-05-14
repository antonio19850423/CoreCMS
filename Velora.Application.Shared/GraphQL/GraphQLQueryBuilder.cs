using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos.GraphQL;

namespace Velora.Application.Shared.GraphQL
{
    public class GraphQLQueryBuilder<TNode>
    {
        private readonly string _fieldName;
        private readonly Dictionary<string, object?> _arguments = new();

        private readonly bool _useCamelCase;

        public GraphQLQueryBuilder(string fieldName, bool useCamelCase = true)
        {
            _fieldName = fieldName;
            _useCamelCase = useCamelCase;
        }

        public GraphQLQueryBuilder<TNode> WithArgument(string name, object? value)
        {
            _arguments[name] = value;
            return this;
        }

        public string BuildQuery()
        {
            var args = _arguments.Any()
                ? "(" + string.Join(", ", _arguments.Select(kvp => $"{kvp.Key}: {SerializeValue(kvp.Value)}")) + ")"
                : "";

            var fields = string.Join("\n", typeof(TNode).GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(GraphQLIgnoreAttribute)))
                .Select(p => _useCamelCase ? ToCamelCase(p.Name) : p.Name));

            var query = $@"
query {{
    {_fieldName}{args} {{
        totalCount
        nodes {{
            {fields}
        }}
        pageInfo {{
            hasNextPage
            endCursor
        }}
    }}
}}";
            return query;
        }
        private string ToCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
                return name;

            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
        private string SerializeValue(object? value)
        {
            if (value == null) return "null";
            if (value is string s) return s.StartsWith("{") ? s : $"\"{s}\"";
            if (value is bool b) return b.ToString().ToLower();
            if (value is GraphQLEnum e) return e.Value;
            if (value.GetType().IsPrimitive) return value.ToString()!;

            // اگر Dictionary باشه
            if (value is IDictionary<string, object> dict)
            {
                var fields = string.Join(", ", dict.Select(kvp => $"{kvp.Key}: {SerializeValue(kvp.Value)}"));
                return $"{{ {fields} }}";
            }

            // اگر anonymous object باشه → با Reflection پراپرتی‌ها رو بگیر
            var props = value.GetType().GetProperties()
                .Where(p => !Attribute.IsDefined(p, typeof(GraphQLIgnoreAttribute)));
            if (props.Any())
            {
                var fields = string.Join(", ", props.Select(p => $"{ToCamelCase(p.Name)}: {SerializeValue(p.GetValue(value))}"));
                return $"{{ {fields} }}";
            }

            return value.ToString()!;
        }

    }

}
