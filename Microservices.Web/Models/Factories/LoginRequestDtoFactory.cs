

namespace Microservices.Web.Models.Factories
{
    public class LoginRequestDtoFactory
    {
        public static LoginRequestDto Create(string userName, string password)
        {
            return new LoginRequestDto
            {
                Password = password,
                UserName = userName
            };
        }
    }
}
