using DataLayer;
using DomainClasses.Dtos;
using DomainClasses.Dtos.Cart;

namespace CartWebApp.Interfaces
{
    public interface ICartService
    {
        public Task<ResponseDto> AddToCart(AddProductToCartDto dto);
        public Task<ResponseDto> ChangeCartDetailQuantity(ChangeCartDetailDto dto);
        public Task<ResponseDto> SetCartDetailDiscountCoupon(ChangeCartDetailDto dto);
        public Task<ResponseDto> SetAllCartDetailDiscountCoupon(ChangeCartDetailDto dto);
        public Task<ResponseDto> RemoveCart(int cartId);
        public Task<ResponseDto> RemoveCartDetail(int detailId);

    }
}
