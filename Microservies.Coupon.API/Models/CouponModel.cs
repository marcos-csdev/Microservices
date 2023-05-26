using System.ComponentModel.DataAnnotations;

namespace Microservies.CouponAPI.Models
{
    public class CouponModel
    {
        [Key]
        public int Id { get; set; }
        [Required] 
        public string CouponCode { get; set; } = string.Empty;
        public double Discount { get; set; }
        /// <summary>
        /// Minimum expenses so the coupon can be used
        /// </summary>
        public int MinAmount { get; set; } 
        public DateTime LastUpdated { get; set; }

    }
}
