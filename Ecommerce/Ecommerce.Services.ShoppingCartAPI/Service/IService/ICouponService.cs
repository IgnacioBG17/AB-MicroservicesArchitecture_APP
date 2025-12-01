using Ecommerce.Services.ShoppingCartAPI.Models.Dto.CouponAPI;

namespace Ecommerce.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
