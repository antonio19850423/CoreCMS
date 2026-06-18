using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISectionGroupItemService : IGenericService<SqlSectionGroupItem, SqlSectionGroupItem, SectionGroupItemDto>, IBaseService
    {
        Task<IQueryable<SectionGroupItemCrud>> GetAllViews();
        Task<ResultDto<SectionGroupItemDto>> CreateAsync(SectionGroupItemCrud input);
        Task<ResultDto<SectionGroupItemDto>> UpdateAsync(SectionGroupItemCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<IEnumerable<ComboBoxItemDto<Guid>>>> GetFooterSectionGroupItemsAsync();
    }
}
