using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IUserOtpService : IGenericService<SqlUserOtp, SqlUserOtp, UserOtpDto>, IBaseService
    {
        Task<IQueryable<UserOtpCrud>> GetAllViews();
        Task<ResultDto<UserOtpDto>> CreateAsync(UserOtpCrud input);
        Task<ResultDto<UserOtpDto>> UpdateAsync(UserOtpCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<UserOtpCrud?> GetLatestOtpAsync(
            string mobile,
            int purpose);
    }
}
