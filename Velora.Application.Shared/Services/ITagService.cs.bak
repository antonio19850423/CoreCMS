using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ITagService : IGenericService<SqlTag, SqlTag, TagDto>, IBaseService
    {
        Task<IQueryable<TagCrud>> GetAllViews();
        Task<ResultDto<TagDto>> CreateAsync(TagCrud input);
        Task<ResultDto<TagDto>> UpdateAsync(TagCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
