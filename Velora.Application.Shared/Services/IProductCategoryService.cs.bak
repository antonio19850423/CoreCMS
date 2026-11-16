using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductCategoryService : IGenericService<SqlProductCategory, SqlProductCategory, ProductCategoryDto>, IBaseService
    {
        Task<IQueryable<ProductCategoryCrud>> GetAllViews();
        Task<ResultDto<ProductCategoryDto>> CreateAsync(ProductCategoryCrud input);
        Task<ResultDto<ProductCategoryDto>> UpdateAsync(ProductCategoryCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<List<ProductCategoryTreeDto>> GetProductCategoryTreeAsync();
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
