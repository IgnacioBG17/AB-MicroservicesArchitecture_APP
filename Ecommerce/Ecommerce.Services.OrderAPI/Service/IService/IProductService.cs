using Ecommerce.Services.OrderAPI.Models.Dto.ProductAPI;

namespace Ecommerce.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
