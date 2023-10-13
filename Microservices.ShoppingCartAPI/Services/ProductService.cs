using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Services.Abstractions;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public async Task<List<ProductDto>> GetProducts()
        {
            var response = await SendMessageAsync($"{StaticDetails.ProductAPIUrl}");

            var products = DeserializeResponseToList<ProductDto>(response);

            return products;
        }
    }
}
