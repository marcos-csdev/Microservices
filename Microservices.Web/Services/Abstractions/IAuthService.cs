using Microservices.Web.Models;

namespace Microservices.Web.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    }
}