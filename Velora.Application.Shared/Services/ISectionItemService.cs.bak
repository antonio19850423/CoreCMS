using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISectionItemService : IGenericService<SqlSectionItem, SqlSectionItem, SectionItemDto>, IBaseService
    {
        Task<IQueryable<SectionItemCrud>> GetAllViews();
        Task<ResultDto<SectionItemDto>> CreateAsync(SectionItemCrud input);
        Task<ResultDto<SectionItemDto>> UpdateAsync(SectionItemCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentSectionItem,
int SectionItemNumber,
int SectionItemSize);
    }
}
