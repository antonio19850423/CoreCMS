using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Enums;
using Velora.Application.Shared.Repositories;

namespace Velora.Application.Shared.Services
{
    public interface IProductTagMappingService : IGenericService<SqlProductTagMapping, SqlProductTagMapping, ProductTagMappingDto>, IBaseService
    {
        Task<ProductTagMappingDto?> GetByProductTagMappingIdAsync(Guid contentItemId, Guid tagId);
        Task<List<ProductTagMappingDto>> GetByProductTagMappingsAsync(Guid ContentItemId);

        Task RemoveAsync(Guid contentItemId, Guid tagId);
    }
}
