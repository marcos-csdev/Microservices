using System.ComponentModel.DataAnnotations;

namespace Microservices.ProductAPI.Models
{
    public class ProductModel
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Range(0, 1000)]
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
