using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductService : IGenericService<SqlProduct, SqlProduct, ProductDto>, IBaseService
    {
        Task<IQueryable<ProductCrud>> GetAllViews();
        Task<ResultDto<ProductDto>> CreateAsync(ProductCrud input);
        Task<ResultDto<ProductDto>> UpdateAsync(ProductCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<ResultDto<ProductListResultDto>> GetProductsAsync(
            int page,
            int pageSize,
            string? categorySlug,
            string? brandSlug,
            string? search,
            string sort,
            decimal? minPrice = null,
            decimal? maxPrice = null);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
