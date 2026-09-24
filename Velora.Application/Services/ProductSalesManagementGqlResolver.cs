using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductSalesManagementGqlResolver : IProductSalesManagementGqlResolver
{
    IProductSalesManagementService _ProductSalesManagementService;
    public ProductSalesManagementGqlResolver(IProductSalesManagementService ProductSalesManagementService)
    {
        _ProductSalesManagementService = ProductSalesManagementService;
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
    [GraphQLName("productSalesManagementView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductSalesManagementCrud>> productSalesManagementView()
    {
        var query = await _ProductSalesManagementService
            .GetAllViewQueryable<SqlProductSalesManagement, SqlProductSalesManagement, ProductSalesManagementCrud>();
        return query.Select(x => new ProductSalesManagementCrud
        {
            ShouldInsert = x.ShouldInsert,
            CouponDiscountAmount = x.CouponDiscountAmount,
            GrossSalesAmount = x.GrossSalesAmount,
            NetSalesAmount = x.NetSalesAmount,
            OrderCount = x.OrderCount,
            ProductDiscountAmount = x.ProductDiscountAmount,
            ProductId = x.ProductId,
            ProductName = x.ProductName??"",
            SoldQuantity= x.SoldQuantity,
            TotalDiscountAmount = x.TotalDiscountAmount,
            VariantId = x.VariantId,
            VariantName = x.VariantName??"",
            Id= x.Id,
            
        });
    }

}
