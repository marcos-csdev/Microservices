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
            return await AddEntityAsync(productDto, $"{StaticDetails.ProductAPIUrl}/create");

        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await GetAllEntitiesAsync(
                $"{StaticDetails.ProductAPIUrl}");

        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await GetEntityByIdAsync(
                $"{StaticDetails.ProductAPIUrl}/GetById/{id}");
        }


        public async Task<ResponseDto?> RemoveProductAsync(int id)
        {
            return await RemoveEntityAsync(
                $"{StaticDetails.ProductAPIUrl}/remove/{id}");
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await UpdateEntityAsync(productDto,
                $"{StaticDetails.ProductAPIUrl}/update");

        }
    }
}
