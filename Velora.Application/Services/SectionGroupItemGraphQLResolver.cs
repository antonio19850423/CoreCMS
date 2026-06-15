using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class SectionGroupItemGqlResolver : ISectionGroupItemGqlResolver
{
    ISectionGroupItemService _SectionGroupItemService;
    public SectionGroupItemGqlResolver(ISectionGroupItemService SectionGroupItemService)
    {
        _SectionGroupItemService = SectionGroupItemService;
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
    /// <returns></returns>
    //[Authorize]
    [GraphQLName("sectionGroupItemView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SectionGroupItemCrud>> sectionGroupItemView() 
    {
        var query = await _SectionGroupItemService
            .GetAllViewQueryable<VwSectionGroupItemForm, VwSectionGroupItemForm, SectionGroupItemCrud>();
        return query.Select(x => new SectionGroupItemCrud
        {
            Id = x.Id,
            Name = x.Name??"",
            SortOrder = x.SortOrder,
            IsActive = x.IsActive,
            Code = x.Code??"",
            Icon = x.Icon??"",
            Color = x.Color??"",
            Description=x.Description??"",
            GroupId=x.GroupId,
            GroupName=x.GroupName??"",
            ShouldInsert = x.ShouldInsert
        });
    }

}
