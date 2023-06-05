using Microservices.AuthAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);

        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<EntityState> RemoveUser(string email);
    }
}
