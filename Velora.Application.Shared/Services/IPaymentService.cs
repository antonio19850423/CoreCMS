using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPaymentService : IGenericService<SqlPayment, SqlPayment, PaymentDto>, IBaseService
    {
        Task<IQueryable<PaymentCrud>> GetAllViews();
        Task<ResultDto<PaymentDto>> CreateAsync(PaymentCrud input);
        Task<ResultDto<PaymentDto>> UpdateAsync(PaymentCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
