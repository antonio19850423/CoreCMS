using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IContentItemService : IGenericService<SqlContentItem, SqlContentItem, ContentItemDto>, IBaseService
    {
        Task<IQueryable<ContentItemCrud>> GetAllViews();
        Task<ResultDto<ContentItemDto>> CreateAsync(ContentItemCrud input);
        Task<ResultDto<ContentItemDto>> UpdateAsync(ContentItemCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
