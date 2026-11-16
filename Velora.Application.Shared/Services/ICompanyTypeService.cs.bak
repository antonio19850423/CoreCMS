using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IComponentTypeService : IGenericService<SqlComponentType, SqlComponentType, ComponentTypeDto>, IBaseService
    {
        Task<IQueryable<ComponentTypeCrud>> GetAllViews();
        Task<ResultDto<ComponentTypeDto>> CreateAsync(ComponentTypeCrud input);
        Task<ResultDto<ComponentTypeDto>> UpdateAsync(ComponentTypeCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
