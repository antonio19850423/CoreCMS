using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IRoleService: IGenericService<SqlRole, PgRole, RoleDto>, IBaseService
    {
        Task<ResultDto<RoleDto>> CreateAsync(RoleCrud input);
        Task<ResultDto<RoleDto>> UpdateAsync(RoleCrud input);
        Task<IEnumerable<RoleDto>> GetByNamesAsync(List<string> roles);
    }
}
