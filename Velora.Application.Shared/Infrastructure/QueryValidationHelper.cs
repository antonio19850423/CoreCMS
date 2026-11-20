using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Infrastructure
{
    public static class QueryValidationHelper
    {
        public static async Task<List<string>> ValidateUniquenessAsync<TEntity>(
       IQueryable<TEntity> query,
       Guid? excludeId = null,
       params (Expression<Func<TEntity, bool>> Predicate, string Message)[] checks)
        {
            var errors = new List<string>();

            foreach (var check in checks)
            {
                var filteredQuery = query;

                // حذف رکورد فعلی در حالت Update
                if (excludeId.HasValue)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "x");
                    var property = Expression.Property(parameter, "Id");

                    var idValue = Expression.Constant(excludeId.Value);

                    var condition = Expression.NotEqual(property, idValue);

                    var idPredicate = Expression.Lambda<Func<TEntity, bool>>(
                        condition,
                        parameter
                    );

                    filteredQuery = filteredQuery.Where(idPredicate);
                }

                if (await filteredQuery.AnyAsync(check.Predicate))
                    errors.Add(check.Message);
            }

            return errors;
        }
    }
}
