using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Velora.Application.Shared.Services
{
    public interface ISmsService:IBaseService
    {
        Task SendOtpAsync(
    string mobile,
    string code,
    int expirationMinutes,
    CancellationToken cancellationToken = default);

        Task<decimal?> GetBalanceAsync(
            CancellationToken cancellationToken = default);
    }
}
