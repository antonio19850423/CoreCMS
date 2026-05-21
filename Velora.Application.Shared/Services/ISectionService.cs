using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ISectionService : IGenericService<SqlSection, SqlSection, SectionDto>, IBaseService
    {
        Task<IQueryable<SectionCrud>> GetAllViews();
        Task<ResultDto<SectionDto>> CreateAsync(SectionCrud input);
        Task<ResultDto<SectionDto>> UpdateAsync(SectionCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentSection,
int SectionNumber,
int SectionSize);
    }
}
