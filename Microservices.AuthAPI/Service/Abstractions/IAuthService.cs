using Microservices.AuthAPI.Models.Dto;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);

        Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto);
    }
}
