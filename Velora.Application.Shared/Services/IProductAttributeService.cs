using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductAttributeService : IGenericService<SqlProductAttribute, SqlProductAttribute, ProductAttributeDto>, IBaseService
    {
        Task<IQueryable<ProductAttributeCrud>> GetAllViews();
        Task<ResultDto<ProductAttributeDto>> CreateAsync(ProductAttributeCrud input);
        Task<ResultDto<ProductAttributeDto>> UpdateAsync(ProductAttributeCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
