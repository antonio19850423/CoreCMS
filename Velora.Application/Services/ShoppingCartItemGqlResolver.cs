using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ShoppingCartItemGqlResolver : IShoppingCartItemGqlResolver
{
    IShoppingCartItemService _ShoppingCartItemService;
    public ShoppingCartItemGqlResolver(IShoppingCartItemService ShoppingCartItemService)
    {
        _ShoppingCartItemService = ShoppingCartItemService;
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
    [GraphQLName("shoppingCartItemView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ShoppingCartItemCrud>> shoppingCartItemView()
    {
        var query = await _ShoppingCartItemService
            .GetAllViewQueryable<SqlShoppingCartItemView, SqlShoppingCartItemView, ShoppingCartItemCrud>();
        return query.Select(x => new ShoppingCartItemCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedAt=x.CreatedAt,
            DiscountAmount = x.DiscountAmount,
            DiscountId = x.DiscountId,
            DiscountItemId = x.DiscountItemId,
            DiscountType = x.DiscountType,
            DiscountTypeTitle = x.DiscountTypeTitle??"",
            DiscountValue = x.DiscountValue,
            FinalUnitPrice = x.FinalUnitPrice,
            OrderCode = x.OrderCode??"",
            ProductBrandName= x.ProductBrandName ?? "",
            ProductCategoryName= x.ProductCategoryName ?? "",
            ProductId = x.ProductId,
            ProductName = x.ProductName??"",
            ProductTypeId= x.ProductTypeId,
            ProductTypeName= x.ProductTypeName??"",
            ProductVariantName= x.ProductVariantName??"",
            ProductVariantPrice= x.ProductVariantPrice,
            Quantity = x.Quantity,
            ShoppingCartId = x.ShoppingCartId,
            ShouldInsert = x.ShouldInsert,
            UnitPrice = x.UnitPrice,
            UpdatedAt = x.UpdatedAt,
            UpdatedAtPersian = x.UpdatedAtPersian??"",
            VariantId = x.VariantId,
        });
    }

}
