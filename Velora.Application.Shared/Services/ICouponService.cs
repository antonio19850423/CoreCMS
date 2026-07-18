using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICouponService : IGenericService<SqlCoupon, SqlCoupon, CouponDto>, IBaseService
    {
        Task<IQueryable<CouponCrud>> GetAllViews();
        Task<ResultDto<CouponDto>> CreateAsync(CouponCrud input);
        Task<ResultDto<CouponDto>> UpdateAsync(CouponCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
    }
}
