using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICmsConfigurationService : IGenericService<SqlCmsConfiguration, SqlCmsConfiguration, CmsConfigurationDto>, IBaseService
    {
        Task<IQueryable<CmsConfigurationCrud>> GetAllViews();
        Task<ResultDto<CmsConfigurationDto>> CreateAsync(CmsConfigurationCrud input);
        Task<ResultDto<CmsConfigurationDto>> UpdateAsync(CmsConfigurationCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
