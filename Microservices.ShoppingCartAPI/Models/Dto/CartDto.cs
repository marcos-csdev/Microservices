namespace Microservices.ShoppingCartAPI.Models.Dto
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; } = null!;
        public int CartHeaderId { get; set; }
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
