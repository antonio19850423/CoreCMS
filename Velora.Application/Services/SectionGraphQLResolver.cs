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
            ContactEmailLabel = x.ContactEmailLabel ??"",
            ContactFirstNameLabel = x.ContactFirstNameLabel ??"",
            ContactLastNameLabel = x.ContactLastNameLabel ??"",
            ContactMessageLabel = x.ContactMessageLabel ??"",
            ContactSubmitButtonText = x.ContactSubmitButtonText ??"",
            CopyrightText = x.CopyrightText ??"",
            MapEmbedUrl = x.MapEmbedUrl ??"",
            BackgroundColor = x.BackgroundColor ??"",
            DescriptionColor = x.DescriptionColor ??"",
            Features= x.Features ??"",
            HeaderColor = x.HeaderColor ??"",
            Icon = x.Icon ??"",
            IconColor = x.IconColor ??"",
            ImageAlt2 = x.ImageAlt2 ??"",
            ImageAlt3 = x.ImageAlt3 ??"",
            ImageAlt4 = x.ImageAlt4 ??"",
            ImageUrl2 = x.ImageUrl2 ??"",
            ImageUrl3 = x.ImageUrl3 ??"",
            ImageUrl4 = x.ImageUrl4 ??"",
            SubtitleColor= x.SubtitleColor ??"",
            ShouldInsert = x.ShouldInsert,
            Link1Url = x.Link1Url ?? "",
            Link1Color = x.Link1Color ?? "",
            Link1TargetId = x.Link1TargetId ?? "",
            Link1Text = x.Link1Text ?? "",
            Link1TypeId = x.Link1TypeId ?? "",
            Link1OpenInNewTab = x.Link1OpenInNewTab,
            Link2Url = x.Link2Url ?? "",
            Link2Color = x.Link2Color ?? "",
            Link2TargetId = x.Link2TargetId ?? "",
            Link2Text = x.Link2Text ?? "",
            Link2TypeId = x.Link2TypeId ?? "",
            Link2OpenInNewTab = x.Link2OpenInNewTab,
            Link3Url = x.Link3Url ?? "",
            Link3Color = x.Link3Color ?? "",
            Link3TargetId = x.Link3TargetId ?? "",
            Link3Text = x.Link3Text ?? "",
            Link3TypeId = x.Link3TypeId ?? "",
            Link3OpenInNewTab = x.Link3OpenInNewTab,
            Link4Url = x.Link4Url ?? "",
            Link4Color = x.Link4Color ?? "",
            Link4TargetId = x.Link4TargetId ?? "",
            Link4Text = x.Link4Text ?? "",
            Link4TypeId = x.Link4TypeId ?? "",
            Link4OpenInNewTab = x.Link4OpenInNewTab,
        });
    }

}
