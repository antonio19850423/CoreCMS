using HotChocolate;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class UserProfileGqlResolver : GqlResolver<SqlUserProfile, PgUserProfile, UserProfileDto>, IGqlResolver
{
    public UserProfileGqlResolver(IUserProfileService service) : base(service) { }

    [GraphQLName("getAllUserProfiles")]
    public override async Task<IQueryable<UserProfileDto>> GetAll()
    {
        return await base.GetAll();
    }
}
