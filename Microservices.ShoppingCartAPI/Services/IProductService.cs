using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
    }
}