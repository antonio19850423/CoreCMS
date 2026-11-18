using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISiteMenuService : IGenericService<SqlSiteMenu, SqlSiteMenu, SiteMenuDto>, IBaseService
    {
        Task<IQueryable<SiteMenuCrud>> GetAllViews();
        Task<ResultDto<SiteMenuDto>> CreateAsync(SiteMenuCrud input);
        Task<ResultDto<SiteMenuCrud>> UpdateAsync(SiteMenuCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    
        Task<SiteMenuCrud?> GetSiteMenuById(Guid id);

        Task<ResultDto<SiteMenuCrud>> CreateAdvancedAsync(SiteMenuCrud input);
}
}
