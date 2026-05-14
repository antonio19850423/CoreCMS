using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IUserService : IGenericService<SqlUser, PgUser, UserDto>, IBaseService
    {
        Task<ResultDto<UserDto>> CreateAsync(UserCrud input);
        Task<ResultDto<UserDto>> UpdateAsync(UserCrud input);
        Task<UserDto?> GetByUserNameAsync(string UserName);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
