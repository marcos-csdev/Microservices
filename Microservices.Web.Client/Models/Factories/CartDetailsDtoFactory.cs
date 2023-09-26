namespace Microservices.Web.Client.Models.Factories
{
    public static class CartDetailsDtoFactory
    {
        public static CartDetailsDto Create(int cartDetailsId, int cartHeaderId,int productId, int count, ProductDto productDto)
        {
            return new CartDetailsDto
            {
                CartDetailsId = cartDetailsId,
                CartHeaderId = cartHeaderId,
                ProductId = productId,
                Count = count,
                ProductDto = productDto
                
            };
        }
        public static CartDetailsDto Create(int productId, int count)
        {
            return new CartDetailsDto
            {
                ProductId = productId,
                Count = count
            };
        }
    }
}
