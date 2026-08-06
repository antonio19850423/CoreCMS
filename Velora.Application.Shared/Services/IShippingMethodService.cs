using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IShippingMethodService : IGenericService<SqlShippingMethod, SqlShippingMethod, ShippingMethodDto>, IBaseService
    {
        Task<IQueryable<ShippingMethodCrud>> GetAllViews();
        Task<ResultDto<ShippingMethodDto>> CreateAsync(ShippingMethodCrud input);
        Task<ResultDto<ShippingMethodDto>> UpdateAsync(ShippingMethodCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
