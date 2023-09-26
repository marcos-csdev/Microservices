namespace Microservices.Web.Client.Models.Factories
{
    public static class CartDtoFactory
    {
        public static CartDto CreateCartDto(CartHeaderDto cartHeader, List<CartDetailsDto> cartDetails)
        {
            return new CartDto
            {
                CartHeader = cartHeader,
                CartDetails = cartDetails
            };
        }
    }
}
