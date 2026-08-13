using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Shared.Services
{
    public interface IShoppingCartService : IGenericService<SqlShoppingCart, SqlShoppingCart, ShoppingCartDto>, IBaseService
    {
        Task<IQueryable<ShoppingCartCrud>> GetAllViews();
        Task<ResultDto<ShoppingCartDto>> CreateAsync(ShoppingCartCrud input);
        Task<ResultDto<ShoppingCartDto>> UpdateAsync(ShoppingCartCrud input);
        Task<ResultDto<BulkInsertResult>> BulkInsertAsync(Stream excelStream);
        Task<byte[]> ExportAsync(
bool exportCurrentPage,
int pageNumber,
int pageSize);
        /// <summary>
        /// دریافت سبد خرید کاربر یا مهمان
        /// </summary>
        Task<ResultDto<ShoppingCartViewDto>> GetCartAsync(
            Guid? userId,
            string? cartToken);



        /// <summary>
        /// اضافه کردن محصول به سبد خرید
        /// </summary>
        Task<ResultDto<ShoppingCartViewDto>> AddAsync(
            Guid? userId,
            string? cartToken,
            ShoppingCartRequestDto input);



        /// <summary>
        /// تغییر تعداد محصول در سبد خرید
        /// </summary>
        Task<ResultDto<ShoppingCartViewDto>> UpdateQuantityAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId,
            int quantity);



        /// <summary>
        /// حذف یک آیتم از سبد خرید
        /// </summary>
        Task<ResultDto<ShoppingCartViewDto>> RemoveAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId);



        /// <summary>
        /// خالی کردن کامل سبد خرید
        /// </summary>
        Task<ResultDto<bool>> ClearAsync(
            Guid? userId,
            string? cartToken);



        /// <summary>
        /// انتقال سبد مهمان به کاربر لاگین شده
        /// </summary>
        Task<ResultDto<ShoppingCartViewDto>> MergeAsync(
            Guid? userId,
            string cartToken);



        /// <summary>
        /// تعداد کل آیتم‌های سبد خرید
        /// </summary>
        Task<ResultDto<int>> GetCountAsync(
            Guid? userId,
            string? cartToken);

        Task<SqlShoppingCart?> GetByIdAsync(Guid shoppingCartId);
        Task<bool> CartHasDiscountAsync(Guid shoppingCartId);
        Task<decimal> GetCartAmountForCouponAsync(
    Guid shoppingCartId);
        Task ApplyCouponToCart(
           ShoppingCart cart,
           Coupon coupon,
           decimal discountAmount);
    }

}
