using System.ComponentModel.DataAnnotations;

namespace Microservices.CouponAPI.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string CouponCode { get; set; } = string.Empty;
        public double Discount { get; set; }
        /// <summary>
        /// Minimum expense to use the coupon
        /// </summary>
        public int MinExpense { get; set; } 
        public DateTime LastUpdated { get; set; }

    }
}
