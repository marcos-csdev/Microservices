using Microservices.ProductAPI.Models.Dtos;

namespace Microservices.ProductAPI.Repositories
{
    public interface IProductRepository
    {
        Task<bool> DelectProductAsync(int productId);
        Task<List<ProductDto>?> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<bool> UpsertProductAsync(ProductDto productDto);
    }
}