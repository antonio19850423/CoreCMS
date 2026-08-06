using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IProductQuestionService : IGenericService<SqlProductQuestion, SqlProductQuestion, ProductQuestionDto>, IBaseService
    {
        Task<IQueryable<ProductQuestionCrud>> GetAllViews();
        Task<ResultDto<ProductQuestionDto>> CreateAsync(ProductQuestionCrud input);
        Task<ResultDto<ProductQuestionDto>> UpdateAsync(ProductQuestionCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
