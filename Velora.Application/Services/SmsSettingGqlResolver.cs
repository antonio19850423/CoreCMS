using Azure;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class SmsSettingGqlResolver : ISmsSettingGqlResolver
{
    ISmsSettingService _SmsSettingService;
    public SmsSettingGqlResolver(ISmsSettingService SmsSettingService)
    {
        _SmsSettingService = SmsSettingService;
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
    [GraphQLName("smsSettingView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SmsSettingCrud>> smsSettingView() 
    {
        var query = await _SmsSettingService
            .GetAllViewQueryable<VwSmsSettingForm, VwSmsSettingForm, SmsSettingCrud>();

        return query.Select(x => new SmsSettingCrud
        {
            Id = x.Id,
            SenderNumber = x.SenderNumber??"",
            ApiKey = x.ApiKey??"",
            CreatedAtPersian = x.CreatedAtPersian ?? "" ,
            CreatedByName = x.CreatedByName ??"",
            IsActive=x.IsActive,
            Provider=x.Provider,
            UpdatedAtPersian=x.UpdatedAtPersian ??"",
            UpdatedByName=x.UpdatedByName ??"",
            ShouldInsert = x.ShouldInsert
        });
    }

}
