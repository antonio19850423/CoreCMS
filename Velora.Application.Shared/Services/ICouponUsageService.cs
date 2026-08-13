using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;

namespace Velora.Application.Shared.Services
{
    public interface ICouponUsageService : IGenericService<SqlCouponUsage, SqlCouponUsage, CouponUsageDto>, IBaseService
    {
        Task<IQueryable<CouponUsageCrud>> GetAllViews();
        Task<ResultDto<CouponUsageDto>> CreateAsync(CouponUsageCrud input);
        Task<ResultDto<CouponUsageDto>> UpdateAsync(CouponUsageCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<ResultDto<ShoppingCartDto>> ApplyCouponAsync(
    Guid shoppingCartId,
    string couponCode);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        Task<ResultDto<ShoppingCartDto>> RemoveCouponAsync(
    Guid shoppingCartId);
        Task<ResultDto<CouponUsageDto>> CreateIfNotExistsAsync(
    Guid couponId,
    Guid orderId,
    Guid? userId);
        Task<ResultDto<bool>> RemoveIfExistsAsync(
    Guid couponId,
    Guid orderId,
    Guid? userId);
    }
}
