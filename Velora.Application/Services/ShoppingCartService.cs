using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Velora.Application.Shared.Dtos;
using Velora.Application.Shared.Services;
using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

namespace Velora.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Velora.EntityFrameworkCore.EntityFramework.SqlServer;

    public class ShoppingCartService : IShoppingCartService, IBaseService
    {

        private readonly IGenericService<
            ShoppingCart,
            ShoppingCart,
            ShoppingCartDto> _shoppingCartService;


        private readonly IGenericService<
            ShoppingCartItem,
            ShoppingCartItem,
            ShoppingCartItemDto> _shoppingCartItemService;


        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly ITransactionService _transactionService;

        public ShoppingCartService(
            IGenericService<
                ShoppingCart,
                ShoppingCart,
                ShoppingCartDto> shoppingCartService,

            IGenericService<
                ShoppingCartItem,
                ShoppingCartItem,
                ShoppingCartItemDto> shoppingCartItemService,

            IMapper mapper, IProductService productService, ITransactionService transactionService)
        {
            _shoppingCartService = shoppingCartService;
            _shoppingCartItemService = shoppingCartItemService;
            _mapper = mapper;
            _productService = productService;
            _transactionService = transactionService;
        }




        public async Task<ResultDto<ShoppingCartViewDto>> GetCartAsync(
            Guid? userId,
            string? cartToken)
        {
            try
            {

                var cartQuery =
                    _shoppingCartService
                    .Query()
                    .Include(x => x.ShoppingCartItems)
                        .ThenInclude(x => x.Product)
                    .Include(x => x.ShoppingCartItems)
                        .ThenInclude(x => x.Variant);



                var cart =
                    await cartQuery
                    .FirstOrDefaultAsync(x =>
                        (userId.HasValue &&
                         x.UserId == userId)

                        ||

                        (!string.IsNullOrEmpty(cartToken)
                         &&
                         x.CartToken == cartToken));



                if (cart == null)
                {
                    return new ResultDto<ShoppingCartViewDto>
                    {
                        Success = true,

                        Data = new ShoppingCartViewDto
                        {
                            Items = new()
                        }
                    };
                }



                var dto = new ShoppingCartViewDto
                {
                    Id = cart.Id,

                    CartToken = cart.CartToken,

                    Items =
                        cart.ShoppingCartItems
                        .Select(item => new ShoppingCartItemViewDto
                        {

                            Id = item.Id,

                            ShoppingCartId =
                                item.ShoppingCartId,


                            ProductId =
                                item.ProductId,


                            VariantId =
                                item.VariantId,


                            ProductName =
                                item.Product.Name,


                            VariantName =
                                item.Variant?.Name,


                            ImageUrl =
                                item.Variant?.Image
                                ??
                                item.Product.MainImage
                                ??
                                item.Product.Thumbnail,


                            UnitPrice =
                                item.UnitPrice,


                            Quantity =
                                item.Quantity,

                        })
                        .ToList()
                };



                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = true,
                    Data = dto
                };


            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }







        public async Task<ResultDto<ShoppingCartViewDto>> AddAsync(
            Guid? userId,
            string? cartToken,
            ShoppingCartRequestDto input)
        {
            try
            {

                var cartResult =
                    await GetOrCreateCartAsync(
                        userId,
                        cartToken);
                await _transactionService.CommitAsync();


                var cart = cartResult;



                var item =
                    await _shoppingCartItemService
                    .Query()
                    .FirstOrDefaultAsync(x =>
                        x.ShoppingCartId == cart.Id
                        &&
                        x.ProductId == input.ProductId
                        &&
                        x.VariantId == input.VariantId);



                if (item != null)
                {

                    item.Quantity += input.Quantity;

                    item.UpdatedAt = DateTime.Now;


                    await _shoppingCartItemService
                        .UpdateAsync(
                            item,
                            item.Id);

                }
                else
                {
                    var product =
                        await _productService
                        .Query()
                        .Include(x => x.ProductVariants)
                        .FirstOrDefaultAsync(x =>
                            x.Id == input.ProductId);


                    if (product == null)
                    {
                        return new ResultDto<ShoppingCartViewDto>
                        {
                            Success = false,
                            Message = "محصول یافت نشد"
                        };
                    }


                    decimal unitPrice = product.Price ?? 0;


                    if (input.VariantId.HasValue)
                    {
                        var variant =
                            product.ProductVariants
                            .FirstOrDefault(x =>
                                x.Id == input.VariantId.Value);


                        if (variant == null)
                        {
                            return new ResultDto<ShoppingCartViewDto>
                            {
                                Success = false,
                                Message = "واریانت انتخاب شده یافت نشد"
                            };
                        }


                        unitPrice = variant.Price;
                    }



                    var newItem = new ShoppingCartItem
                    {
                        Id = Guid.NewGuid(),

                        ShoppingCartId =
                            cart.Id,

                        ProductId =
                            input.ProductId,

                        VariantId =
                            input.VariantId,

                        Quantity =
                            input.Quantity,

                        UnitPrice =
                            unitPrice
                    };


                    await _shoppingCartItemService
                        .CreateAsync(
                            _mapper.Map<ShoppingCartItemDto>(newItem));
                }

                await _transactionService.CommitAsync();

                return await GetCartAsync(
                    userId,
                    cartToken);

            }
            catch (Exception ex)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }









        public async Task<ResultDto<ShoppingCartViewDto>> UpdateQuantityAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId,
            int quantity)
        {

            var item =
                await _shoppingCartItemService
                .Query()
                .FirstOrDefaultAsync(x => x.Id == itemId);



            if (item == null)
            {
                return new ResultDto<ShoppingCartViewDto>
                {
                    Success = false,
                    Message = "آیتم پیدا نشد"
                };
            }


            item.Quantity = quantity;

            item.UpdatedAt = DateTime.Now;



            await _shoppingCartItemService
                .UpdateAsync(
                    item,
                    item.Id);



            return await GetCartAsync(
                userId,
                cartToken);

        }









        public async Task<ResultDto<ShoppingCartViewDto>> RemoveAsync(
            Guid? userId,
            string? cartToken,
            Guid itemId)
        {

            var item =
                await _shoppingCartItemService
                .Query()
                .FirstOrDefaultAsync(x => x.Id == itemId);



            if (item != null)
            {

                await _shoppingCartItemService
                    .DeleteAsync(item.Id);

            }



            return await GetCartAsync(
                userId,
                cartToken);

        }









        public async Task<ResultDto<bool>> ClearAsync(
            Guid? userId,
            string? cartToken)
        {

            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            if (cart != null)
            {

                var ids =
                    cart.ShoppingCartItems
                    .Select(x => x.Id)
                    .ToList();


                foreach (var id in ids)
                {
                    await _shoppingCartItemService
                        .DeleteAsync(id);
                }

            }



            return new ResultDto<bool>
            {
                Success = true,
                Data = true
            };

        }









        public async Task<ResultDto<ShoppingCartViewDto>> MergeAsync(
            Guid userId,
            string cartToken)
        {

            var guestCart =
                await _shoppingCartService
                .Query()
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x =>
                    x.CartToken == cartToken);



            if (guestCart == null)
                return await GetCartAsync(userId, null);



            guestCart.UserId = userId;

            guestCart.UpdateAt = DateTime.Now;



            await _shoppingCartService
                .UpdateAsync(
                    _mapper.Map<ShoppingCartDto>(guestCart),
                    guestCart.Id);



            return await GetCartAsync(
                userId,
                cartToken);

        }









        public async Task<ResultDto<int>> GetCountAsync(
            Guid? userId,
            string? cartToken)
        {

            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            return new ResultDto<int>
            {
                Success = true,

                Data =
                    cart?
                    .ShoppingCartItems
                    .Sum(x => x.Quantity)
                    ?? 0
            };

        }









        private async Task<ShoppingCart> GetOrCreateCartAsync(
            Guid? userId,
            string? cartToken)
        {

            var cart =
                await GetCartEntityAsync(
                    userId,
                    cartToken);



            if (cart != null)
                return cart;



            var entity = new ShoppingCart
            {

                Id = Guid.NewGuid(),

                UserId = (userId==Guid.Empty?null:userId),

                CartToken =
                    cartToken
                    ??
                    Guid.NewGuid()
                    .ToString(),


                Status = 1

            };


            await _shoppingCartService
                .CreateAsync(
                    _mapper.Map<ShoppingCartDto>(entity));



            return entity;

        }







        private async Task<ShoppingCart?> GetCartEntityAsync(
            Guid? userId,
            string? cartToken)
        {

            return await _shoppingCartService
                .Query()
                .Include(x => x.ShoppingCartItems)
                .FirstOrDefaultAsync(x =>
                    (userId.HasValue &&
                     x.UserId == userId)

                    ||

                    (!string.IsNullOrEmpty(cartToken)
                     &&
                     x.CartToken == cartToken));

        }

    }
}
