using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductReviewGqlResolver : IProductReviewGqlResolver
{
    IProductReviewService _ProductReviewService;
    public ProductReviewGqlResolver(IProductReviewService ProductReviewService)
    {
        _ProductReviewService = ProductReviewService;
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
    [GraphQLName("productReviewView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductReviewCrud>> productReviewView()
    {
        var query = await _ProductReviewService
            .GetAllViewQueryable<SqlProductReviewView, SqlProductReviewView, ProductReviewCrud>();
        return query.Select(x => new ProductReviewCrud
        {
            Id = x.Id,
            Title = x.Title ??"",
            Comment = x.Comment ??"",
            Rate = x.Rate ,
            ProductId = x.ProductId ,
            UserId = x.UserId ,
            IsApproved = x.IsApproved ,
            ProductSlug = x.ProductSlug??"" ,
            ProductTitle = x.ProductTitle ??"",
            ShouldInsert = x.ShouldInsert,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName=x.CreatedByName ??"",
            UpdatedAtPersian= x.UpdatedAtPersian ?? "",
            UpdatedByName = x.UpdatedByName ?? "",

        });
    }

}
