using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPageTemplateComponentService : IGenericService<SqlPageTemplateComponent, SqlPageTemplateComponent, PageTemplateComponentDto>, IBaseService
    {
        Task<IQueryable<PageTemplateComponentCrud>> GetAllViews();
        Task<ResultDto<PageTemplateComponentDto>> CreateAsync(PageTemplateComponentCrud input);
        Task<ResultDto<PageTemplateComponentDto>> UpdateAsync(PageTemplateComponentCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
