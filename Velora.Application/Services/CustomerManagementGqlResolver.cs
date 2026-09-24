using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class CustomerManagementGqlResolver : ICustomerManagementGqlResolver
{
    ICustomerManagementService _CustomerManagementService;
    public CustomerManagementGqlResolver(ICustomerManagementService CustomerManagementService)
    {
        _CustomerManagementService = CustomerManagementService;
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
    [GraphQLName("customerManagementView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<CustomerManagementCrud>> customerManagementView()
    {
        var query = await _CustomerManagementService
            .GetAllViewQueryable<SqlCustomerManagement, SqlCustomerManagement, CustomerManagementCrud>();
        return query.Select(x => new CustomerManagementCrud
        {
            CouponDiscountAmount=x.CouponDiscountAmount,
            CustomerName=x.CustomerName??"",
            GrossSalesAmount=x.GrossSalesAmount,    
            LastOrderAt=x.LastOrderAt,
            LastOrderAtPersian=x.LastOrderAtPersian??"",
            NetSalesAmount=x.NetSalesAmount,
            ShouldInsert=x.ShouldInsert,
            OrderCount=x.OrderCount,
            ProductDiscountAmount=x.ProductDiscountAmount,
            SalesAfterProductDiscount= x.SalesAfterProductDiscount,
            SoldQuantity=x.SoldQuantity,
            TotalDiscountAmount=x.TotalDiscountAmount,
            UserId= x.UserId,
            Id= x.Id,
            
            
        });
    }

}
