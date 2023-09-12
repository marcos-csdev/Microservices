using Microservices.AuthAPI.Models;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IJwtTokenGenerator
    {
        /// <summary>
        /// Generates a token from the user object
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles">a user can have more than one role</param>
        /// <returns></returns>
        string GenerateToken(MSUser user, IEnumerable<string> roles);
    }
}
