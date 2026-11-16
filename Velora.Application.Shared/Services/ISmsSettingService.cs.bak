using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISmsSettingService : IGenericService<SqlSmsSetting, SqlSmsSetting, SmsSettingDto>, IBaseService
    {
        Task<IQueryable<SmsSettingCrud>> GetAllViews();
        Task<ResultDto<SmsSettingDto>> CreateAsync(SmsSettingCrud input);
        Task<ResultDto<SmsSettingDto>> UpdateAsync(SmsSettingCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
