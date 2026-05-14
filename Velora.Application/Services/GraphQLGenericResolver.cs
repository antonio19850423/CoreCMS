using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;

public  class GqlResolver<TEntitySql, TEntityPg, TDto>
    where TEntitySql : class
    where TEntityPg : class
    where TDto : class
{
    private readonly IGenericService<TEntitySql, TEntityPg, TDto> _service;

    public GqlResolver(IGenericService<TEntitySql, TEntityPg, TDto> service)
    {
        _service = service;
    }
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public virtual async Task<IQueryable<TDto>> GetAll()
    {
        return await _service.GetAllQuery();
    }
    public async Task<ResultDto<TDto?>> GetById(Guid id)
    {
        return await _service.GetByIdAsync(id);
    }

    public async Task<ResultDto<TDto>> Create(TDto input)
    {
        return await _service.CreateAsync(input);
    }

    public async Task<ResultDto<TDto?>> Update(TDto input, params object[] idies)
    {
        return await _service.UpdateAsync(input, idies);
    }

    public async Task<ResultDto<bool>> Delete(Guid id)
    {
        return await _service.DeleteAsync(id);
    }
}
