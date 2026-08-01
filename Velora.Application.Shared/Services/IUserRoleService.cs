using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IUserRoleService : IGenericService<SqlUserRole, PgUserRole, UserRoleDto>, IBaseService
    {
        Task<List<RoleDto>> GetRolesByUserIdAsync(Guid userId);
        Task<IQueryable<UserRoleViewDto>> GetPgUserRolesView();
        Task<UserRoleDto?> GetByUserIdAsync(Guid UserId);
        Task<IQueryable<UserRoleViewDto>> GetSqlUserRolesView();
        Task<UserRoleDto?> GetByUserRoleIdAsync(Guid UserId, Guid RoleId);
        Task<List<UserRoleDto>> GetByUserRolesAsync(Guid userId);
        Task<RoleDto?> GetRoleByCodeAsync(
    string roleCode);    }

}
