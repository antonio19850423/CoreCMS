using Azure;
using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;
[ExtendObjectType("Query")]
public class SmsLogGqlResolver : ISmsLogGqlResolver
{
    ISmsLogService _SmsLogService;
    public SmsLogGqlResolver(ISmsLogService SmsLogService)
    {
        _SmsLogService = SmsLogService;
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
    [GraphQLName("smsLogView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<SmsLogCrud>> smsLogView() 
    {
        var query = await _SmsLogService
            .GetAllViewQueryable<VwSmsLogForm, VwSmsLogForm, SmsLogCrud>();

        return query.Select(x => new SmsLogCrud
        {
            Id = x.Id,
            SmsType = x.SmsType??"",
            ProviderMessageId = x.ProviderMessageId??"",
            CreatedAtPersian = x.CreatedAtPersian ?? "" ,
            ErrorMessage=x.ErrorMessage ??"",
            Message = x.Message ??  "",
            IsSuccess=x.IsSuccess,
            Mobile=x.Mobile??"",
            Provider=x.Provider,
            SentAtPersian=x.SentAtPersian ??"",
            ShouldInsert = x.ShouldInsert
        });
    }

}
