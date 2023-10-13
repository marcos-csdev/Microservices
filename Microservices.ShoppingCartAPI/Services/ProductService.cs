using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Services.Abstractions;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory, StaticDetails.ProductAPIName)
        {
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var response = await SendMessageAsync( $"{StaticDetails.ProductAPIUrl}");

            var products = DeserializeResponseToList<ProductDto>(response);

            return products;
        }
    }
}
