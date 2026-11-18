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
        Task<ResultDto<SectionCrud>> UpdateAsync(SectionCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentSection,
int SectionNumber,
int SectionSize);
    
        Task<SectionCrud?> GetSectionById(Guid id);

        Task<ResultDto<SectionCrud>> CreateAdvancedAsync(SectionCrud input);
}
}
