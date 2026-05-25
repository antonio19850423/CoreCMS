using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISiteSettingService : IGenericService<SqlSiteSetting, SqlSiteSetting, SiteSettingDto>, IBaseService
    {
        Task<IQueryable<SiteSettingCrud>> GetAllViews();
        Task<ResultDto<SiteSettingDto>> CreateAsync(SiteSettingCrud input);
        Task<ResultDto<SiteSettingDto>> UpdateAsync(SiteSettingCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
