using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface IDiscountService : IGenericService<SqlDiscount, SqlDiscount, DiscountDto>, IBaseService
    {
        Task<IQueryable<DiscountCrud>> GetAllViews();
        Task<ResultDto<DiscountDto>> CreateAsync(DiscountCrud input);
        Task<ResultDto<DiscountDto>> UpdateAsync(DiscountCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<List<ActiveDiscountDto>> GetActiveDiscountsAsync();
        DiscountCalculationResultDto CalculateDiscount(
            DiscountCalculationInput input,
            IReadOnlyList<ActiveDiscountDto> activeDiscounts);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
