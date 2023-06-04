using Microservices.AuthAPI.Models;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(MSUser user);
    }
}
