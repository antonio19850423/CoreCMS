using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class UserRoleGqlResolver : GqlResolver<SqlUserRole, PgUserRole, UserRoleDto>, IGqlResolver
{
    IUserRoleService _service;
    public UserRoleGqlResolver(IUserRoleService service) : base(service) {
        _service=service;
    }

    [GraphQLName("getAllUserRoles")]
    public override async Task<IQueryable<UserRoleDto>> GetAll()
    {
        return await base.GetAll();
    }


    [GraphQLName("GetPgUserRolesView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<UserRoleViewDto>> GetPgUserRolesView()
    {
        return _service.GetPgUserRolesView();
    }

    [GraphQLName("GetSqlUserRolesView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<UserRoleViewDto>> GetSqlUserRolesView()
    {
        return _service.GetSqlUserRolesView();
    }



}
