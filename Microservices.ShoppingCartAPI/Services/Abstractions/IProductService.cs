using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Services.Abstractions
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetProducts();
    }
}