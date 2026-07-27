using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Services;
using Velora.Application.Shared;
using Velora.Application.Shared.Attributes;
using Velora.Application.Shared.Constants;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Infrastructure;
using Velora.Application.Shared.Services;


namespace Velora.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICookieService _cookieService;
        private readonly ICurrentUserService _currentUserService;
        
        public ShoppingCartController(
            IShoppingCartService shoppingCartService, ICookieService cookieService, ICurrentUserService currentUserService)
        {
            _shoppingCartService = shoppingCartService;
            _cookieService = cookieService;
            _currentUserService = currentUserService;
        }



        /// <summary>
        /// دریافت سبد خرید
        /// برای کاربر لاگین شده یا مهمان
        /// </summary>
        [HttpGet]
        [Route("GetCartAsync")]
        public async Task<IActionResult> GetCartAsync(
            Guid? userId,
            string? cartToken)
        {
            var result =
                await _shoppingCartService
                .GetCartAsync(
                    userId,
                    cartToken
                );

            return Ok(result);
        }




        /// <summary>
        /// اضافه کردن محصول به سبد
        /// </summary>
        [HttpPost]
        [Route("AddAsync")]
        public async Task<IActionResult> AddAsync(
            [FromBody] ShoppingCartRequestDto input)
        {

            var userId = _currentUserService.GetUserId();



            var cartToken =
                _cookieService
                .GetOrCreate(
                    CookieKeys.CartToken,
                    () => Guid.NewGuid().ToString());



            var result =
                await _shoppingCartService.AddAsync(
                    userId,
                    cartToken,
                    input);



            return Ok(result);
        }





        /// <summary>
        /// تغییر تعداد محصول
        /// </summary>
        [HttpPut]
        [Route("UpdateQuantityAsync")]
        public async Task<IActionResult> UpdateQuantityAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId,
            int quantity)
        {


            var result =
                await _shoppingCartService
                .UpdateQuantityAsync(
                    userId,
                    cartToken,
                    itemId,
                    quantity
                );


            return Ok(result);
        }








        /// <summary>
        /// حذف یک آیتم از سبد
        /// </summary>
        [HttpDelete]
        [Route("RemoveAsync")]
        public async Task<IActionResult> RemoveAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId)
        {


            var result =
                await _shoppingCartService
                .RemoveAsync(
                    userId,
                    cartToken,
                    itemId
                );


            return Ok(result);
        }









        /// <summary>
        /// حذف کل سبد خرید
        /// </summary>
        [HttpDelete]
        [Route("ClearAsync")]
        public async Task<IActionResult> ClearAsync(
            Guid? userId,
            string? cartToken)
        {

            var result =
                await _shoppingCartService
                .ClearAsync(
                    userId,
                    cartToken
                );


            return Ok(result);
        }









        /// <summary>
        /// انتقال سبد مهمان بعد از Login
        /// </summary>
        [HttpPost]
        [Route("MergeAsync")]
        public async Task<IActionResult> MergeAsync(
            Guid userId,
            string cartToken)
        {


            var result =
                await _shoppingCartService
                .MergeAsync(
                    userId,
                    cartToken
                );


            return Ok(result);
        }









        /// <summary>
        /// تعداد آیتم های سبد برای Header
        /// </summary>
        [HttpGet]
        [Route("GetCountAsync")]
        public async Task<IActionResult> GetCountAsync(
            Guid? userId,
            string? cartToken)
        {

            var result =
                await _shoppingCartService
                .GetCountAsync(
                    userId,
                    cartToken
                );


            return Ok(result);
        }

    }
}
