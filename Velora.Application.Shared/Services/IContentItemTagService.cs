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
    public interface IContentItemTagService : IGenericService<SqlContentItemTag, SqlContentItemTag, ContentItemTagDto>, IBaseService
    {
        Task<ContentItemTagDto?> GetByContentItemTagIdAsync(Guid contentItemId, Guid tagId);
        Task<List<ContentItemTagDto>> GetByContentItemTagsAsync(Guid ContentItemId);

        Task RemoveAsync(Guid contentItemId, Guid tagId);
    }
}
