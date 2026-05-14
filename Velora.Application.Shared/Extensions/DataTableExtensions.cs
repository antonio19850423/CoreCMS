using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Reflection;

    public static class DataTableExtensions
    {
        public static List<T> ToModelList<T>(this DataTable table) where T : new()
        {
            var result = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var propMap = props
                .Where(p => table.Columns.Contains(p.Name))
                .ToDictionary(p => p.Name);

            foreach (DataRow row in table.Rows)
            {
                var obj = new T();

                foreach (var (name, prop) in propMap)
                {
                    var rawValue = row[name];
                    var converted = rawValue.ConvertTo(prop.PropertyType);
                    prop.SetValue(obj, converted);
                }

                result.Add(obj);
            }

            return result;
        }
    }



}
