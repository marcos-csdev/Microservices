using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Client.Models
{
    public class LoginRequestDto
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
