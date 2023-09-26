namespace Microservices.Web.Client.Models
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }

        public ProductDto? ProductDto { get; set; } = null;
    }
}
