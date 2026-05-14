using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Repositories
{
    public interface IReadRepository<TEntity> where TEntity : class
    {
        Task<IQueryable<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllQueryable();
        IQueryable<TView> GetAllViewQueryable<TView>() where TView : class;
        Task<IList<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> GetByIdAsync(params object[] keyValues);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> ReadOnly();
        Task<IQueryable<TEntity>> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    }
}
