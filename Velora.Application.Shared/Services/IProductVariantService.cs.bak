using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductVariantService : IGenericService<SqlProductVariant, SqlProductVariant, ProductVariantDto>, IBaseService
    {
        Task<IQueryable<ProductVariantCrud>> GetAllViews();
        Task<ResultDto<ProductVariantDto>> CreateAsync(ProductVariantCrud input);
        Task<ResultDto<ProductVariantDto>> UpdateAsync(ProductVariantCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
