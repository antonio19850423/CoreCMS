using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductBrandService : IGenericService<SqlProductBrand, SqlProductBrand, ProductBrandDto>, IBaseService
    {
        Task<IQueryable<ProductBrandCrud>> GetAllViews();
        Task<ResultDto<ProductBrandDto>> CreateAsync(ProductBrandCrud input);
        Task<ResultDto<ProductBrandDto>> UpdateAsync(ProductBrandCrud input);
        Task<ResultDto<List<ProductBrandOptionDto>>> GetProductBrandsAsync();
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
