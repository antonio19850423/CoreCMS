using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.Application.Shared.Extensions;
using HotChocolate.Authorization;

[ExtendObjectType("Query")]
public class RoleGqlResolver:IRoleGqlResolver
    {
    IRoleService _roleService;
    public RoleGqlResolver(IRoleService roleService)  {
        _roleService = roleService;
    }
    [Authorize]
    [GraphQLName("roleView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<RoleCrud>> roleView()
        {
        var result = await _roleService.GetAllViewQueryable<PgRole,SqlRole,RoleCrud>();
        return result;
        }

    }
