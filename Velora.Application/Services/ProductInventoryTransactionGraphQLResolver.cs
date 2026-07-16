using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ProductInventoryTransactionGqlResolver : IProductInventoryTransactionGqlResolver
{
    IProductInventoryTransactionService _ProductInventoryTransactionService;
    public ProductInventoryTransactionGqlResolver(IProductInventoryTransactionService ProductInventoryTransactionService)
    {
        _ProductInventoryTransactionService = ProductInventoryTransactionService;
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
    [GraphQLName("productInventoryTransactionView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ProductInventoryTransactionCrud>> productInventoryTransactionView()
    {
        var query = await _ProductInventoryTransactionService
            .GetAllViewQueryable<SqlProductInventoryTransactionView, SqlProductInventoryTransactionView, ProductInventoryTransactionCrud>();
        return query.Select(x => new ProductInventoryTransactionCrud
        {
            Id = x.Id,
            ParentId = x.ParentId,
            ProductId = x.ProductId,
            ProductName = x.ProductName??"",
            ReferenceId = x.ReferenceId,
            ReferenceDetailId = x.ReferenceDetailId,
            ReasonId = x.ReasonId,
            Note=x.Note??"",
            OperationType = x.OperationType,
            OperationTypeName = x.OperationTypeName??"",
            ProductVariantId = x.ProductVariantId,
            ProductVariantName= x.ProductVariantName??"",
            ChangeQuantity = x.ChangeQuantity,
            CreatedAtPersian = x.CreatedAtPersian??"",
            CreatedByName= x.CreatedByName??"",
            ReasonCode=x.ReasonCode??"",
            ReasonName=x.ReasonName??"",
            ShouldInsert = x.ShouldInsert   
        });
    }

}
