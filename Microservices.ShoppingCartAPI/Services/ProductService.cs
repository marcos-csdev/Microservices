using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class ProductService : BaseService
    {
        public ProductService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var response = await SendMessageAsync("ShoppingCartAPIClient", StaticDetails.ProductAPIURL);

            var products = DeserializeResponseToList<ProductDto>(response);

            return products;
        }
    }
}
