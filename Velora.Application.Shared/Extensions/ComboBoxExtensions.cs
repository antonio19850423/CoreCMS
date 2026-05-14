using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Extensions
{
    public static class ComboBoxExtensions
    {
        public static Task<List<ComboBoxItemDto<TKey>>> ToComboBoxItemsAsync<TDto, TKey>(
            this IQueryable<TDto> query,
            Expression<Func<TDto, TKey>> valueSelector,
            Expression<Func<TDto, string>> labelSelector)
        {
            return query
                .Select(x => new ComboBoxItemDto<TKey>
                {
                    Value = valueSelector.Compile()(x),
                    Label = labelSelector.Compile()(x)
                })
                .ToListAsync();
        }
    }

}
