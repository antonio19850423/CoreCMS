using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IDiscountItemService : IGenericService<SqlDiscountItem, SqlDiscountItem, DiscountItemDto>, IBaseService
    {
        Task<IQueryable<DiscountItemCrud>> GetAllViews();
        Task<ResultDto<DiscountItemDto>> CreateAsync(DiscountItemCrud input);
        Task<ResultDto<DiscountItemDto>> UpdateAsync(DiscountItemCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
