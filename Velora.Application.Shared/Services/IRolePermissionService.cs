using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IRolePermissionService : IGenericService<SqlRolePermission, PgRolePermission, RolePermissionDto>, IBaseService
    {
        Task<List<RolePermissionDto>> GetByPermissionRolesAsync(Guid permissionId);
        Task<RolePermissionDto?> GetByPermissionRoleIdAsync(Guid PermissionId, Guid RoleId);
        Task<List<RolePermissionMapDto>> GetRolePermissionMapAsync();
        Task RemoveAsync(Guid permissionId, Guid roleId);
    }
}
