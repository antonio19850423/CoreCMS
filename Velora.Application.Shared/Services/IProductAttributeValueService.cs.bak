using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductAttributeValueService : IGenericService<SqlProductAttributeValue, SqlProductAttributeValue, ProductAttributeValueDto>, IBaseService
    {
        Task<IQueryable<ProductAttributeValueCrud>> GetAllViews();
        Task<ResultDto<ProductAttributeValueDto>> CreateAsync(ProductAttributeValueCrud input);
        Task<ResultDto<ProductAttributeValueDto>> UpdateAsync(ProductAttributeValueCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
