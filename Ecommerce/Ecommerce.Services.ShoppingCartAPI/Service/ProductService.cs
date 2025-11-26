using Ecommerce.Services.ShoppingCartAPI.Models.Dto;
using Ecommerce.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Ecommerce.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync("/api/product");

            if (!response.IsSuccessStatusCode)
                return new List<ProductDto>();

            var apiContent = await response.Content.ReadAsStringAsync();

            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp == null || !resp.IsSuccess)
                return new List<ProductDto>();

            return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.Result));
        }

    }
}
