using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPaymentStatusLogService : IGenericService<SqlPaymentStatusLog, SqlPaymentStatusLog, PaymentStatusLogDto>, IBaseService
    {
        Task<IQueryable<PaymentStatusLogCrud>> GetAllViews();
        Task<ResultDto<PaymentStatusLogDto>> CreateAsync(PaymentStatusLogCrud input);
        Task<ResultDto<PaymentStatusLogDto>> UpdateAsync(PaymentStatusLogCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
