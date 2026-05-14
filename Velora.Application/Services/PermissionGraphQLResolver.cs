using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.PostgreSQL;
[ExtendObjectType("Query")]
public class PermissionGqlResolver : IPermissionGqlResolver
{
    IPermissionService _PermissionService;
    public PermissionGqlResolver(IPermissionService PermissionService)
    {
        _PermissionService = PermissionService;
    }
    [Authorize]
    [GraphQLName("permissionView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<PermissionCrud>> permissionView()
    {
        var query = await _PermissionService
            .GetAllViewQueryable<PgPermissionView, SqlPermissionView, PermissionCrud>();

        return query.Select(x => new PermissionCrud
        {
            Id = x.Id,
            ResourceId = x.ResourceId,
            ResourceName = x.ResourceName,
            ResourceCode = x.ResourceCode,
            ResourceTypeCode = x.ResourceTypeCode,
            Actions = x.Actions,

            RoleIds = x.RoleIds ?? "",           // 🔴 حیاتی
            RoleNames = x.RoleNames,
            IsActive = x.IsActive ?? false,      // 🔴 حیاتی

            CreatedAt = x.CreatedAt,
            UpdatedAt = x.UpdatedAt,
            CreatedByName = x.CreatedByName,
            UpdatedByName = x.UpdatedByName,
            ShouldInsert = x.ShouldInsert
        });
    }

}
