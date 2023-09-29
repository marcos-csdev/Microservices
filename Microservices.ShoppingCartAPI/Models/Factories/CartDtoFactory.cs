using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI.Models.Factories
{
    public static class CartDtoFactory
    {
        public static CartDto Create(CartHeaderDto cartHeader, List<CartDetailsDto> cartDetails)
        {
            return new CartDto
            {
                CartHeader = cartHeader,
                CartDetails = cartDetails
            };
        }
    }
}
