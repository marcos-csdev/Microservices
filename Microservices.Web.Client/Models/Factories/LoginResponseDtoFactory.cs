
namespace Microservices.Web.Client.Models.Factories
{
    public static class LoginResponseDtoFactory
    {
        public static LoginResponseDto Create(MSUserDto user, string token)
        {
            return new LoginResponseDto
            {
                User = user,
                Token = token
            };
        }
    }
}
