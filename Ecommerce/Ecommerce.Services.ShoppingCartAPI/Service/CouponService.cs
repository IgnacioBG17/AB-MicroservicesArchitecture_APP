using Ecommerce.Services.ShoppingCartAPI.Models.Dto;
using Ecommerce.Services.ShoppingCartAPI.Models.Dto.CouponAPI;
using Ecommerce.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Ecommerce.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupon/GetByCode{couponCode}");

            if (!response.IsSuccessStatusCode)
                return new CouponDto();

            var apiContent = await response.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp == null || !resp.IsSuccess)
                return new CouponDto();

            return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(resp.Result));
        }
    }
}
