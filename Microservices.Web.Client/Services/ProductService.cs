using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IMessageService messageService) : base(messageService)
        {
        }

        public async Task<ResponseDto?> AddProductAsync(ProductDto productDto)
        {
            return await AddEntityAsync(productDto, $"{StaticDetails.ProductAPIUrl}/api/products/create");

        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await GetAllEntitiesAsync(
                $"{StaticDetails.ProductAPIUrl}/api/products");

        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await GetEntityByIdAsync(id,
                $"{StaticDetails.ProductAPIUrl}/api/products/GetById/{id}");
        }


        public async Task<ResponseDto?> RemoveProductAsync(int id)
        {
            return await RemoveEntityAsync(id,
                $"{StaticDetails.ProductAPIUrl}/api/products/remove/{id}");
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await UpdateEntityAsync(productDto,
                $"{StaticDetails.ProductAPIUrl}/api/products/update");

        }
    }
}
