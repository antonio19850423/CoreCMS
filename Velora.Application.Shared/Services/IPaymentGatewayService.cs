using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPaymentGatewayService : IGenericService<SqlPaymentGateway, SqlPaymentGateway, PaymentGatewayDto>, IBaseService
    {
        Task<IQueryable<PaymentGatewayCrud>> GetAllViews();
        Task<ResultDto<PaymentGatewayDto>> CreateAsync(PaymentGatewayCrud input);
        Task<ResultDto<PaymentGatewayDto>> UpdateAsync(PaymentGatewayCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
