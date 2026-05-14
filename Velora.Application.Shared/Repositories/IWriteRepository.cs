using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Repositories
{
    public interface IWriteRepository<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);
        Task<bool> RemoveAsync(TEntity entity);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> DeleteRangeAsync(IList<TEntity> entities);
        Task<bool> UpdateAttachAsync(TEntity entity);
        Task<int> CommitAsync();
    }
}
