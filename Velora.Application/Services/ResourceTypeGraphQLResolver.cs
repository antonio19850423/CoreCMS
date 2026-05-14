using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ResourceTypeGqlResolver : IResourceTypeGqlResolver
{
    IResourceTypeService _resourceTypeService;
    public ResourceTypeGqlResolver(IResourceTypeService resourceTypeService)
    {
        _resourceTypeService = resourceTypeService;
    }

    [Authorize]
    [GraphQLName("resourceTypeView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ResourceTypeCrud>> resourceTypeView()
    {
        var result = await _resourceTypeService.GetAllViewQueryable<PgResourcetype, SqlResourceType, ResourceTypeCrud>();
        return result;
    }

}
