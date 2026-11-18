using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IContentCategoryService : IGenericService<SqlContentCategory, SqlContentCategory, ContentCategoryDto>, IBaseService
    {
        Task<IQueryable<ContentCategoryCrud>> GetAllViews();
        Task<ResultDto<ContentCategoryDto>> CreateAsync(ContentCategoryCrud input);
        Task<ResultDto<ContentCategoryCrud>> UpdateAsync(ContentCategoryCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    
        Task<ContentCategoryCrud?> GetContentCategoryById(Guid id);

        Task<ResultDto<ContentCategoryCrud>> CreateAdvancedAsync(ContentCategoryCrud input);
}
}
