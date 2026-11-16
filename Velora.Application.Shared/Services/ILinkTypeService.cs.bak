using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ILinkTypeService : IGenericService<SqlLinkType, SqlLinkType, LinkTypeDto>, IBaseService
    {
        Task<IQueryable<LinkTypeCrud>> GetAllViews();
        Task<ResultDto<LinkTypeDto>> CreateAsync(LinkTypeCrud input);
        Task<ResultDto<LinkTypeDto>> UpdateAsync(LinkTypeCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<List<LinkTypeDto>>> AddRangeAsync(List<LinkTypeDto> inputs);
    }
}
