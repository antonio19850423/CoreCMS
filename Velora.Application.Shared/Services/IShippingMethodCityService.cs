using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IShippingMethodCityService : IGenericService<SqlShippingMethodCity, SqlShippingMethodCity, ShippingMethodCityDto>, IBaseService
    {
        Task<IQueryable<ShippingMethodCityCrud>> GetAllViews();
        Task<ResultDto<ShippingMethodCityDto>> CreateAsync(ShippingMethodCityCrud input);
        Task<ResultDto<ShippingMethodCityDto>> UpdateAsync(ShippingMethodCityCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
