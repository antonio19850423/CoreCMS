using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class InventoryManagementGqlResolver : IInventoryManagementGqlResolver
{
    IInventoryManagementService _InventoryManagementService;
    public InventoryManagementGqlResolver(IInventoryManagementService InventoryManagementService)
    {
        _InventoryManagementService = InventoryManagementService;
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
    [GraphQLName("inventoryManagementView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<InventoryManagementCrud>> inventoryManagementView()
    {
        var query = await _InventoryManagementService
            .GetAllViewQueryable<SqlInventoryManagement, SqlInventoryManagement, InventoryManagementCrud>();
        return query.Select(x => new InventoryManagementCrud
        {
           VariantName = x.VariantName??"",
           VariantId = x.VariantId,
           ProductName = x.ProductName??"",
           IsOutOfStock = x.IsOutOfStock,
           Id = x.Id,
           Barcode = x.Barcode??"",
           CurrentQuantity = x.CurrentQuantity,
           InventoryInQuantity = x.InventoryInQuantity,
           InventoryStatusPriority = x.InventoryStatusPriority,
           InventoryStatusTitle = x.InventoryStatusTitle??"",
           InventoryValue = x.InventoryValue,
           IsInStock = x.IsInStock,
           IsLowStock = x.IsLowStock,
           LowStockQuantity = x.LowStockQuantity,
           LowStockThreshold = x.LowStockThreshold,
           PaidQuantity = x.PaidQuantity,
           Price = x.Price,
           ProductIsActive = x.ProductIsActive,
           ReservedQuantity = x.ReservedQuantity,
           ShouldInsert = x.ShouldInsert,
          Sku= x.Sku??"",
          VariantIsActive = x.VariantIsActive
            
            
        });
    }

}
