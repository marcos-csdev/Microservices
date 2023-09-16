using Microservices.ShoppingCartAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microservices.ShoppingCartAPI.Models
{
    public class CartDetailsModel
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeaderModel? CartHeader { get; set; } = null!;
        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; } = null!;
        public int Count { get; set; }
    }
}
