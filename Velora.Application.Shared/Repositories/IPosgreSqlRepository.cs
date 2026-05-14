using Microsoft.AspNetCore.OData.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Repositories
{
    public interface IPosgreSqlRepository<TEntity> : IReadRepository<TEntity>, IWriteRepository<TEntity> where TEntity : class
    {
        Task ExecuteSqlAsync(string sql, params object[] parameters);
        Task<IList<T>> ExecuteSqlQueryAsync<T>(string sql, params object[] parameters) where T : class;
        Task<T> ExecuteScalarAsync<T>(string sql, params object[] parameters);
        IQueryable<TView> GetViewQueryable<TView>() where TView : class;
        Task<(IList<TView> Data, int Count)> GetViewWithODataAsync<TView>(ODataQueryOptions<TView> queryOptions) where TView : class;
        Task<List<TResult>> GetListAsync<TResult>(
    Expression<Func<TEntity, bool>> predicate,
    Expression<Func<TEntity, TResult>> selector,
    params Expression<Func<TEntity, object>>[] includes) where TResult : class;
        Task<bool> AnyRelatedAsync<TEntity>(Guid id) where TEntity : class;

    }
}
