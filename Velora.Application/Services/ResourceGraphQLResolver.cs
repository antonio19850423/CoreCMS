using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ResourceGqlResolver : IResourceGqlResolver
{
    IResourceService _resourceService;
    public ResourceGqlResolver(IResourceService resourceService)
    {
        _resourceService = resourceService;
    }
    [Authorize]
    [GraphQLName("resourceView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ResourceCrud>> resourceView()
    {
        var result = await _resourceService.GetAllViewQueryable<PgResourceFormView, SqlResourceFormView, ResourceCrud>();
        return result;
    }
}
