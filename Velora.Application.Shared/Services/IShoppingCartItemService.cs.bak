using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IShoppingCartItemService : IGenericService<SqlShoppingCartItem, SqlShoppingCartItem, ShoppingCartItemDto>, IBaseService
    {
        Task<IQueryable<ShoppingCartItemCrud>> GetAllViews();
        Task<ResultDto<ShoppingCartItemDto>> CreateAsync(ShoppingCartItemCrud input);
        Task<ResultDto<ShoppingCartItemDto>> UpdateAsync(ShoppingCartItemCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
