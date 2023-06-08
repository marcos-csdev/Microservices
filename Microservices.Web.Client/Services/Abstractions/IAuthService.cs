using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services.Abstractions
{
    public interface IAuthService
    {
        Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto);
    }
}