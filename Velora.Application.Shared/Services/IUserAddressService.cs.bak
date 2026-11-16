using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IUserAddressService : IGenericService<SqlUserAddress, SqlUserAddress, UserAddressDto>, IBaseService
    {
        Task<IQueryable<UserAddressCrud>> GetAllViews();
        Task<ResultDto<UserAddressDto>> CreateAsync(UserAddressCrud input);
        Task<ResultDto<UserAddressDto>> UpdateAsync(UserAddressCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<IEnumerable<UserAddressDto>>> GetUserAddressesAsync();
        Task<ResultDto<UserAddressDto?>> GetUserAddressByIdAsync(
      Guid addressId);
    }
}
