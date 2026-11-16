using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IPermissionService : IGenericService<SqlPermission, PgPermission, PermissionDto>, IBaseService
    {
        Task<ResultDto<PermissionDto>> CreateAsync(PermissionCrud input);
        Task<ResultDto<PermissionDto>> UpdateAsync(PermissionCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<PermissionDto?> GetByResourceIdAsync(Guid resourceId);
        Task<HashSet<string>> GetAllowedMenuResourceIdsAsync();
    }
}
