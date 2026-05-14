using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IResourceTypeService:IGenericService<SqlResourceType, PgResourcetype, ResourceTypeDto>, IBaseService
    {
        Task<ResultDto<ResourceTypeDto>> CreateAsync(ResourceTypeCrud input);
        Task<ResultDto<ResourceTypeDto>> UpdateAsync(ResourceTypeCrud input);
    }

}
