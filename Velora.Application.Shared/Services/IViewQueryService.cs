using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface IViewQueryService: IBaseService
    {
        Task<List<TResult>> GetListAsync<TView, TResult>()
            where TView : class
            where TResult : class;

        Task<TResult?> FirstOrDefaultAsync<TView, TResult>(
            Expression<Func<TResult, bool>> predicate)
            where TView : class
            where TResult : class;

        Task<IQueryable<TResult>> Query<TView, TResult>()
            where TView : class
            where TResult : class;
    }
}
