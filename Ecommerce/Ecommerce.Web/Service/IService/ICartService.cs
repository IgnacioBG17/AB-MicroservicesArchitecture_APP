using Ecommerce.Web.Models;
using Ecommerce.Web.Models.ShoppingCart;

namespace Ecommerce.Web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string userId);
        Task<ResponseDto> UpsertCartAsync(CartDto cartDto);
        Task<ResponseDto> RemoveFromCartAsync(int cartDetailsId);
        Task<ResponseDto> ApplyCouponAsync(CartDto cartDto);
        Task<ResponseDto> EmailCart(CartDto cartDto);
    }
}
