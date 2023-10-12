namespace Microservices.Web.Client.Models
{
    public class CartWrapperDto
    {
        public CartDto? CartDto { get; set; }
        public List<CouponDto>? CouponsList { get; set;}
    }
}
