using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Extensions
{
    public static class ExcelTypeConverter
    {
        public static object? ConvertTo(this object? value, Type targetType)
        {
            if (value == null || value == DBNull.Value)
                return null;

            var stringValue = value.ToString();
            if (string.IsNullOrWhiteSpace(stringValue))
                return null;

            var type = Nullable.GetUnderlyingType(targetType) ?? targetType;

            try
            {
                if (type == typeof(Guid))
                    return Guid.Parse(stringValue);

                if (type == typeof(int))
                    return int.Parse(stringValue);

                if (type == typeof(bool))
                    return bool.Parse(stringValue);

                if (type == typeof(DateTime))
                    return DateTime.Parse(stringValue);

                return Convert.ChangeType(stringValue, type);
            }
            catch
            {
                throw new InvalidCastException(
                    $"Cannot convert '{stringValue}' to {type.Name}");
            }
        }
    }

}
