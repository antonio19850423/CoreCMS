using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;


namespace Velora.Application.Shared.Extensions
{

    public static class IQueryableExtensions
    {
        public static IQueryable SelectWithDefaults(this IQueryable source, Type sourceType, Type destinationType)
        {
            var parameter = Expression.Parameter(sourceType, "x");
            var bindings = new List<MemberBinding>();

            foreach (var destProp in destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var sourceProp = sourceType.GetProperty(destProp.Name);
                if (sourceProp == null) continue;

                var sourceAccess = Expression.Property(parameter, sourceProp);
                Expression valueExpr;

                if (!IsNullableType(destProp.PropertyType) && IsNullableType(sourceProp.PropertyType))
                {
                    var defaultValue = Expression.Default(destProp.PropertyType);
                    valueExpr = Expression.Coalesce(sourceAccess, defaultValue);
                }
                else
                {
                    valueExpr = sourceAccess;
                }

                bindings.Add(Expression.Bind(destProp, valueExpr));
            }

            var body = Expression.MemberInit(Expression.New(destinationType), bindings);
            var lambda = Expression.Lambda(body, parameter);

            var selectMethod = typeof(Queryable).GetMethods()
                .First(m => m.Name == "Select" && m.GetParameters().Length == 2)
                .MakeGenericMethod(sourceType, destinationType);

            return (IQueryable)selectMethod.Invoke(null, new object[] { source, lambda });
        }

        private static bool IsNullableType(Type type) =>
            !type.IsValueType || (Nullable.GetUnderlyingType(type) != null);
    }

}
