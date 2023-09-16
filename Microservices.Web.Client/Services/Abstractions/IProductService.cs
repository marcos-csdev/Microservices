using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IProductService
    {
        Task<ResponseDto?> AddProductAsync(ProductDto productDto);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> RemoveProductAsync(int id);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
    }
}