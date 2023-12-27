using DomainClasses.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using DomainClasses.Dtos;
using CartWebApp.Interfaces;
using Utilities.Cart;
using DomainClasses.Dtos.Cart;
using DomainClasses.Entities.Enum;

namespace CartWebApp.Service
{
    public class CartService : ICartService
    {
        public async Task<ResponseDto> AddToCart(AddProductToCartDto dto)
        {
            await using (var context = new AppDbContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
                if (product == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "محصول پیدا نشد"
                };

                if (product.Quantity <= 0) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "محصول در انبار موجود نمی باشد"
                };
                var cart = await context.Carts.FirstOrDefaultAsync(c => c.CartId == dto.CartId);
                // اگر سبد خرید جاری ای برای این کاربر وجود نداشت یک سبد خرید جدید ساخته میشود
                if (cart != null)
                {
                    var cartDetail = await context.CartDetails.FirstOrDefaultAsync(d => d.CartId == cart.CartId && d.ProductId == product.Id);


                    // اگر ریز سبد خرید (آیتم های درون سبد) ای با این عنوان وجود نداشت به قبلی ها اضافه در غیر اینصورت یک رکورد جدید ساخته میشود
                    if (cartDetail != null)
                    {
                        if (product.Quantity < cartDetail.Quantity + dto.Quantity) return new ResponseDto() { IsSuccess = false, StatusCode = 500, Message = $"فقط {product.Quantity} تعداد از محصول در انبار موجود است" };
                    }
                    else
                    {
                        await context.CartDetails.AddAsync(new CartDetail
                        {
                            CartId = cart.CartId,
                            ProductId = product.Id,
                            Price = (dto.Discount > 0 ? (dto.DiscountType == DiscountType.Price ? CartUtility.CalculateCartItemDiscountByPrice(product.Price, dto.Discount) : CartUtility.CalculateCartItemDiscountByPercent(product.Price, dto.Discount)) : product.Price),
                            Quantity = dto.Quantity,
                            DiscountType = dto.DiscountType

                    });
                    }
                }
                else
                {
                    cart = new Cart
                    {
                        CartId = new Guid()
                    };
                    await context.Carts.AddAsync(cart);
                    await context.CartDetails.AddAsync(new CartDetail
                    {
                        CartId = cart.CartId,
                        ProductId = product.Id,
                        Price = (dto.Discount > 0 ? (dto.DiscountType == DiscountType.Price ? CartUtility.CalculateCartItemDiscountByPrice(product.Price, dto.Discount) : CartUtility.CalculateCartItemDiscountByPercent(product.Price, dto.Discount)): product.Price),
                        Quantity = dto.Quantity,
                        DiscountType = dto.DiscountType
                    });
                }

                await context.SaveChangesAsync();

                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }


        }
        public async Task<ResponseDto> ChangeCartDetailQuantity(ChangeCartDetailDto dto)
        {
            await using (var context = new AppDbContext())
            {
                var cartDetail = await context.CartDetails
                    .Where(f => f.DetailId == dto.CartDetailId)
                    .Include(f => f.Product)
                   .FirstOrDefaultAsync();
                if (cartDetail == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "ایتم سبد پیدا نشد"
                };
                if (dto.IsAddQuantity)
                {
                    dto.Quantity += 1;
                    cartDetail.Quantity += 1;
                }
                else
                {
                    dto.Quantity -= 1;
                    cartDetail.Quantity -= 1;
                }

                if (cartDetail.Product.Quantity < cartDetail.Quantity) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "موجودی محصول کافی نیست"
                };

                // عدم اجازه کم تر شدن از 0
                if (cartDetail.Quantity <= 0)
                {
                    cartDetail.Quantity += 1;
                    dto.Quantity += 1;
                }

                await context.SaveChangesAsync();

                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }


        }
        public async Task<ResponseDto> SetCartDetailDiscountCoupon(ChangeCartDetailDto dto)
        {
            await using (var context = new AppDbContext())
            {
                var cartDetail = await context.CartDetails
                    .Where(f => f.DetailId == dto.CartDetailId)
                    .Include(f => f.Product)
                    .FirstOrDefaultAsync();
                if (cartDetail == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "ایتم سبد پیدا نشد"
                };

                cartDetail.Discount = dto.Discount;
                cartDetail.Price = (dto.DiscountType == DiscountType.Price ? CartUtility.CalculateCartItemDiscountByPrice(cartDetail.Price, dto.Discount) : CartUtility.CalculateCartItemDiscountByPercent(cartDetail.Price, dto.Discount));
                cartDetail.DiscountType = dto.DiscountType;

                await context.SaveChangesAsync();

                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }


        }
        public async Task<ResponseDto> SetAllCartDetailDiscountCoupon(ChangeCartDetailDto dto)
        {
            await using (var context = new AppDbContext())
            {
                var cart = await context.Carts.Include(c => c.CartDetails).FirstOrDefaultAsync(c => c.CartId == dto.CartId);
                if (cart == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "سبد پیدا نشد"
                };
                foreach (var cartDetail in cart.CartDetails)
                {
                    cartDetail.Discount = dto.Discount;
                    cartDetail.Price = (dto.DiscountType == DiscountType.Price ? CartUtility.CalculateCartItemDiscountByPrice(cartDetail.Price, dto.Discount) : CartUtility.CalculateCartItemDiscountByPercent(cartDetail.Price, dto.Discount));
                    cartDetail.DiscountType = dto.DiscountType;
                }
                await context.SaveChangesAsync();
                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }


        }
        public async Task<ResponseDto> RemoveCart(int cartId)
        {
            await using (var context = new AppDbContext())
            {
                var cart = await context.Carts.FindAsync(cartId);
                if (cart == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "سبد پیدا نشد"
                };
                context.Remove(cart);
                await context.SaveChangesAsync();
                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }

        }
        public async Task<ResponseDto> RemoveCartDetail(int detailId)
        {
            await using (var context = new AppDbContext())
            {
                var cartDetail = await context.CartDetails.FindAsync(detailId);
                if (cartDetail == null) return new ResponseDto()
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "ایتم سبد پیدا نشد"
                };
                context.Remove(cartDetail);
                await context.SaveChangesAsync();
                return new ResponseDto()
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "عملیات با موفقیت انجام شد"
                };
            }

        }
    }
}
