using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IStateService : IGenericService<SqlState, SqlState, StateDto>, IBaseService
    {
        Task<IQueryable<StateCrud>> GetAllViews();
        Task<ResultDto<StateDto>> CreateAsync(StateCrud input);
        Task<ResultDto<StateDto>> UpdateAsync(StateCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
