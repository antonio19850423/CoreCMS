using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IResourceService : IGenericService<SqlResource, PgResource, ResourceDto>, IBaseService
    {
        Task<IQueryable<ResourcesViewDto>> GetAllViews();
        Task<List<ResourcesViewDto>> GetAllMenusAsync(string languageCode);
        Task<ResultDto<ResourceDto>> CreateAsync(ResourceCrud input);
        Task<ResultDto<ResourceDto>> UpdateAsync(ResourceCrud input);
        Task<byte[]> ExportAsync(
    bool exportCurrentPage,
    int pageNumber,
    int pageSize);
        Task<ResourceDto?> GetByCodeAsync(string code);
    }
}
