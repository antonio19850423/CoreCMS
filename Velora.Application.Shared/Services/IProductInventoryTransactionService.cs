using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductInventoryTransactionService : IGenericService<SqlProductInventoryTransaction, SqlProductInventoryTransaction, ProductInventoryTransactionDto>, IBaseService
    {
        Task<IQueryable<ProductInventoryTransactionCrud>> GetAllViews();
        Task<ResultDto<ProductInventoryTransactionDto>> CreateAsync(ProductInventoryTransactionCrud input);
        Task<ResultDto<ProductInventoryTransactionDto>> UpdateAsync(ProductInventoryTransactionCrud input);
        Task<int> GetAvailableQuantityAsync(
    Guid productId,
    Guid? productVariantId = null);
        Task<Dictionary<Guid, int>> GetInventoryAsync(
            List<Guid> productIds);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<int> GetInventoryAsync(
            Guid productId,
            Guid? productVariantId = null);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
