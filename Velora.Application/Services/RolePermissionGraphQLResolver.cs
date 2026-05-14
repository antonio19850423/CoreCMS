using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class RolePermissionGqlResolver : GqlResolver<SqlRolePermission, PgRolePermission, RolePermissionDto>, IGqlResolver
{
    public RolePermissionGqlResolver(IRolePermissionService service) : base(service) { }

    [GraphQLName("getAllRolePermissions")]
    public override async Task<IQueryable<RolePermissionDto>> GetAll()
    {
        return await base.GetAll();
    }
}
