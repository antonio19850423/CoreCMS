using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Infrastructure
{
    public static class EnumHelper
    {
        public static string GetDisplayName(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field?
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault() as DisplayAttribute;

            return attribute?.Name ?? value.ToString();
        }


        public static IEnumerable<ComboBoxItemDto<int>> GetComboItems<T>()
            where T : Enum
        {
            return Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(e => new ComboBoxItemDto<int>
                {
                    Value = Convert.ToInt32(e),
                    Label = GetDisplayName(e)
                });
        }
        public static IEnumerable<ComboBoxItemDto<TValue>> GetComboItems<TEnum, TValue>()
    where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new ComboBoxItemDto<TValue>
                {
                    Value = (TValue)Convert.ChangeType(e, typeof(TValue)),
                    Label = GetDisplayName(e)
                });
        }
    }
}
