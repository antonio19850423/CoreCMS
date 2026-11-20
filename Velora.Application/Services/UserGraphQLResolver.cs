using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Extensions;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

[ExtendObjectType("Query")]
public class UserGqlResolver :IUserGqlResolver
    {
    IUserService _userService;
    public UserGqlResolver(IUserService userService)  {
        _userService = userService;
    }
    [Authorize]
    [GraphQLName("userView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public  async Task<IQueryable<UserCrud>> userView()
    {
        var query = await _userService
    .GetAllViewQueryable<PgUserDetailView, SqlUserDetailView, UserCrud>();

        var result= query.Select(x => new UserCrud
        {
            Id = x.Id,
            MobileNumber = x.MobileNumber??"",
            Email = x.Email??"",
            IsActive = x.IsActive,
            Address = x.Address??"",
            Age=x.Age,
            CityId=x.CityId,
            CityName=x.CityName??"",
            CountryId= x.CountryId,
            CountryName= x.CountryName??"",
            CreatedAt = x.CreatedAt,
            FirstName= x.FirstName??"",
            IsDeleted= x.IsDeleted,
            LastName= x.LastName??"",
            NationalCode=x.NationalCode??"",
            Password=x.Password??"",
            PhoneNumber=x.PhoneNumber??"",
            ProfileImage=x.ProfileImage??"",
            RoleId=x.RoleId,
            RoleIds=x.RoleIds??"",
            RoleName=x.RoleName??"",
            RoleNames=x.RoleNames??"",
            ShouldInsert=x.ShouldInsert,
            StateId=x.StateId,
            StateName=x.StateName??"",
            UpdatedAt=x.UpdatedAt,
            UserName = x.UserName ?? "",
        });
       
        return result.OrderByDescending(c=>c.UpdatedAt??c.CreatedAt);
    }
}
