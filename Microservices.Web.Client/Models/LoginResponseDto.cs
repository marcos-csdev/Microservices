namespace Microservices.Web.Client.Models
{
    public class LoginResponseDto
    {
        public MSUserDto User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
