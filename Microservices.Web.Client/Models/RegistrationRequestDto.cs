using System.ComponentModel.DataAnnotations;

namespace Microservices.Web.Client.Models
{
    public class RegistrationRequestDto
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string? Role { get; set; } = string.Empty;
    }
}
