using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPageTemplateService : IGenericService<SqlPageTemplate, SqlPageTemplate, PageTemplateDto>, IBaseService
    {
        Task<IQueryable<PageTemplateCrud>> GetAllViews();
        Task<ResultDto<PageTemplateDto>> CreateAsync(PageTemplateCrud input);
        Task<ResultDto<PageTemplateCrud>> UpdateAsync(PageTemplateCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    
        Task<PageTemplateCrud?> GetPageTemplateById(Guid id);

        Task<ResultDto<PageTemplateCrud>> CreateAdvancedAsync(PageTemplateCrud input);
}
}
