using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICategoryAttributeService : IGenericService<SqlCategoryAttribute, SqlCategoryAttribute, CategoryAttributeDto>, IBaseService
    {
        Task<IQueryable<CategoryAttributeCrud>> GetAllViews();
        Task<ResultDto<CategoryAttributeDto>> CreateAsync(CategoryAttributeCrud input);
        Task<ResultDto<CategoryAttributeDto>> UpdateAsync(CategoryAttributeCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
