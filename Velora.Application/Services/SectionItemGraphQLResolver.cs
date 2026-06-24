using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class SectionItemGqlResolver : ISectionItemGqlResolver
{
    ISectionItemService _SectionItemService;
    public SectionItemGqlResolver(ISectionItemService SectionItemService)
    {
        _SectionItemService = SectionItemService;
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
    [GraphQLName("sectionItemView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SectionItemCrud>> sectionItemView()
    {
        var query = await _SectionItemService
            .GetAllViewQueryable<SqlSectionItemView, SqlSectionItemView, SectionItemCrud>();
        return query.Select(x => new SectionItemCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            IsActive = x.IsActive,
            ImageUrl = x.ImageUrl??"",
            Description = x.Description??"",
            SortOrder = x.SortOrder,
            CreatedAtPersian = x.CreatedAtPersian ?? "",
            Subtitle = x.Subtitle ?? "",
            Title= x.Title ?? "",
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            IconAlt = x.IconAlt ??"",
            ImageAlt = x.ImageAlt ??"",
            Price = x.Price ??"",
            IconColor = x.IconColor ??"",
            AvatarUrl = x.AvatarUrl ??"",
            AvatarAlt = x.AvatarAlt ??"",
            BackgroundColor = x.BackgroundColor ??"",
            DescriptionColor = x.DescriptionColor ??"",
            Icon= x.Icon??"",
            SubtitleColor = x.SubtitleColor ??"",
            TitleColor= x.TitleColor ??"",
            ComponentTypeName = x.ComponentTypeName ??"",
            Features= x.Features ??"",
            Answer  =x.Answer ??"",
            Name = x.Name ??"",
            Question = x.Question ??"",
            Role = x.Role ??"",
            SectionGroupItemName = x.SectionGroupItemName ??"",
            SectionGroupItemId  =x.SectionGroupItemId,
            Link1Url = x.Link1Url ??"",
            Link1Color = x.Link1Color ??"",
            Link1TargetId = x.Link1TargetId ,
            Link1Text = x.Link1Text ??"",
            Link1TypeId = x.Link1TypeId,
            Link1OpenInNewTab = x.Link1OpenInNewTab,
            Link2Url = x.Link2Url ?? "",
            Link2Color = x.Link2Color ?? "",
            Link2TargetId = x.Link2TargetId,
            Link2Text = x.Link2Text ?? "",
            Link2TypeId = x.Link2TypeId,
            Link2OpenInNewTab = x.Link2OpenInNewTab,
            Link3Url = x.Link3Url ?? "",
            Link3Color = x.Link3Color ?? "",
            Link3TargetId = x.Link3TargetId,
            Link3Text = x.Link3Text ?? "",
            Link3TypeId = x.Link3TypeId ,
            Link3OpenInNewTab = x.Link3OpenInNewTab,
            Link4Url = x.Link4Url ?? "",
            Link4Color = x.Link4Color ?? "",
            Link4TargetId = x.Link4TargetId,
            Link4Text = x.Link4Text ?? "",
            Link4TypeId = x.Link4TypeId,
            Link4OpenInNewTab = x.Link4OpenInNewTab,
            ShouldInsert = x.ShouldInsert
        });
    }

}
