using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.Application.Shared.Extensions;
using HotChocolate.Authorization;

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
        var result = await _userService.GetAllViewQueryable<PgUserDetailView, SqlUserDetailView, UserCrud>();
        return result;
    }
}
