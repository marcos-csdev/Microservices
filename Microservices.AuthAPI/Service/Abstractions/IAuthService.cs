using Microservices.AuthAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IAuthService
    {
        /// <summary>
        /// Returns an error message as string
        /// </summary>
        /// <param name="registrationRequestDto"></param>
        /// <returns></returns>
        Task<string> Register(RegistrationRequestDto registrationRequestDto);

        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<bool> RemoveUser(string email);
    }
}
