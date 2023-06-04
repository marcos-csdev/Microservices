using Microservices.AuthAPI.Models.Dto;
using System.Runtime.InteropServices;

namespace Microservices.AuthAPI.Models.Factories
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
