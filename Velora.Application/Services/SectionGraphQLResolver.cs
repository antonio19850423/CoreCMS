using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class SectionGqlResolver : ISectionGqlResolver
{
    ISectionService _SectionService;
    public SectionGqlResolver(ISectionService SectionService)
    {
        _SectionService = SectionService;
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
    [Authorize]
    [GraphQLName("sectionView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SectionCrud>> sectionView()
    {
        var query = await _SectionService
            .GetAllViewQueryable<SqlSectionView, SqlSectionView, SectionCrud>();
        return query.Select(x => new SectionCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            ImageUrl = x.ImageUrl??"",
            Description = x.Description??"",
            ComponentTypeId = x.ComponentTypeId,
            ColumnsCount = x.ColumnsCount??0,
            SortOrder = x.SortOrder,
            ComponentTypeName = x.ComponentTypeName ?? "",
            CreatedAtPersian = x.CreatedAtPersian ?? "",
            Subtitle = x.Subtitle ?? "",
            Title= x.Title ?? "",
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            IconAlt = x.IconAlt ??"",
            ImageAlt = x.ImageAlt ??"",
            Link1Text = x.Link1Text ??"",
            Link2Text = x.Link2Text ??"",
            Link3Text = x.Link3Text ??"",
            Link4Text = x.Link4Text ??"",
            ShouldInsert = x.ShouldInsert
        });
    }

}
