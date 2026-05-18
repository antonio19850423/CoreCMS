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
    /// <summary>
    /// قوانین کلی GraphQL Resolver:
    /// - نام کلاس باید به GqlResolver ختم شود
    /// - نام Query باید به صورت EntityName + View و به شکل camelCase باشد
    /// - تمام فیلدهای nullable باید مقدار پیش‌فرض داشته باشند (جلوگیری از null)
    /// - View باید از مدل Sql<Entity>View استفاده کند
    /// - Entity و View باید در globalUsing.cs ثبت شده باشند
    /// - در تنظیمات GraphQL باید از AddTypeExtension استفاده شود
    /// - منطق بیزینسی داخل Resolver قرار نگیرد (فقط Mapping و Query)
    /// - عملیات Read/List فقط از طریق GraphQL انجام می‌شود (نه Service)
    /// </summary>
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
