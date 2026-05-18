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
