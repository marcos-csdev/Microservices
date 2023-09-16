using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<bool> DelectProductAsync(int productId);
        Task<List<ProductDto>?> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<bool> UpsertProductAsync(ProductDto productDto);
    }
}