namespace Microservices.Web.Models
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string CouponCode { get; set; } = string.Empty;
        public double Discount { get; set; }
        public int MinExpense { get; set; }
    }
}
