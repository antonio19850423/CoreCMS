using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IInventoryTransactionReasonService : IGenericService<SqlInventoryTransactionReason, SqlInventoryTransactionReason, InventoryTransactionReasonDto>, IBaseService
    {
        Task<IQueryable<InventoryTransactionReasonCrud>> GetAllViews();
        Task<ResultDto<InventoryTransactionReasonDto>> CreateAsync(InventoryTransactionReasonCrud input);
        Task<ResultDto<InventoryTransactionReasonDto>> UpdateAsync(InventoryTransactionReasonCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
