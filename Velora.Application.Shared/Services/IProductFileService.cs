using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductFileService : IGenericService<SqlProductFile, SqlProductFile, ProductFileDto>, IBaseService
    {
        Task<IQueryable<ProductFileCrud>> GetAllViews();
        Task<ResultDto<ProductFileDto>> CreateAsync(ProductFileCrud input);
        Task<ResultDto<ProductFileDto>> UpdateAsync(ProductFileCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
