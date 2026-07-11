using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductTagService : IGenericService<SqlProductTag, SqlProductTag, ProductTagDto>, IBaseService
    {
        Task<IQueryable<ProductTagCrud>> GetAllViews();
        Task<ResultDto<ProductTagDto>> CreateAsync(ProductTagCrud input);
        Task<ResultDto<ProductTagDto>> UpdateAsync(ProductTagCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
