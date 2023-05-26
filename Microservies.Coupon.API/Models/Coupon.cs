namespace Microservies.Coupon.API.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public double Discount { get; set; }
        public int MinAmount { get; set; } 
        public DateTime LastUpdated { get; set; }

    }
}
