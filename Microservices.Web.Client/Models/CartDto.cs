namespace Microservices.Web.Client.Models
{
    public class CartDto
    {
        public CartHeaderDto? CartHeader { get; set; } = null!;
        public IEnumerable<CartDetailsDto>? CartDetails { get; set; }
    }
}
