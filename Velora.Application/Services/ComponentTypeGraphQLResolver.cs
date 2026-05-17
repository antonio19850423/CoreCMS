using HotChocolate;
using HotChocolate.Authorization;
using HotChocolate.Types;
using Velora.Application.Services;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
[ExtendObjectType("Query")]
public class ComponentTypeGqlResolver : IComponentTypeGqlResolver
{
    IComponentTypeService _ComponentTypeService;
    public ComponentTypeGqlResolver(IComponentTypeService ComponentTypeService)
    {
        _ComponentTypeService = ComponentTypeService;
    }
    [Authorize]
    [GraphQLName("componentTypeView")]
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public async Task<IQueryable<ComponentTypeCrud>> componentTypeView()
    {
        var query = await _ComponentTypeService
            .GetAllViewQueryable<SqlComponentTypeView, SqlComponentTypeView, ComponentTypeCrud>();
        return query.Select(x => new ComponentTypeCrud
        {
            Id = x.Id,
            Code = x.Code,
            Name = x.Name,
            CreatedAtPersian = x.CreatedAtPersian??"",
            Description = x.Description,
            IsActive = x.IsActive??false,
            Type = x.Type,
            UpdatedAtPersian= x.UpdatedAtPersian??"",
            CreatedByName = x.CreatedByName ?? "",
            UpdatedByName = x.UpdatedByName ?? "",
            ShouldInsert = x.ShouldInsert
        });
    }

}
