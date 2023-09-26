using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Client.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string? ImageLocalPath { get; set; }
        [Range(1, 100)]
        public int Count { get; set; } = 1;
        
    }
}
