using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductTypeService : IGenericService<SqlProductType, SqlProductType, ProductTypeDto>, IBaseService
    {
        Task<IQueryable<ProductTypeCrud>> GetAllViews();
        Task<ResultDto<ProductTypeDto>> CreateAsync(ProductTypeCrud input);
        Task<ResultDto<ProductTypeDto>> UpdateAsync(ProductTypeCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
