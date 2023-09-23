using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var response = await SendMessageAsync(StaticDetails.CartAPIName, $"{StaticDetails.ProductAPIFullUrl}");

            var products = DeserializeResponseToList<ProductDto>(response);

            return products;
        }
    }
}
