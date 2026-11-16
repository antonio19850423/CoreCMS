using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISmsLogService : IGenericService<SqlSmsLog, SqlSmsLog, SmsLogDto>, IBaseService
    {
        Task<IQueryable<SmsLogCrud>> GetAllViews();
        Task<ResultDto<SmsLogDto>> CreateAsync(SmsLogCrud input);
        Task<ResultDto<SmsLogDto>> UpdateAsync(SmsLogCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
