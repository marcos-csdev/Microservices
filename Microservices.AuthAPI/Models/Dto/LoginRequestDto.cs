namespace Microservices.AuthAPI.Models.Dto
{
    public class LoginRequestDto
    {

        public string Password { get; set; } = string.Empty;

        public string UserName { get; set;} = string.Empty;
    }
}
