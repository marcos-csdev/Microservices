using Microservices.Web.Models;
using Microservices.Web.Models.Factories;
using Microservices.Web.Services.Abstractions;
using Microservices.Web.Utility;

namespace Microservices.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await SendRequest(loginRequestDto, "login");
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await SendRequest(registrationRequestDto, "register");
        }

        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await SendRequest(registrationRequestDto, "assignRole");
        }

        private async Task<ResponseDto?> SendRequest(RegistrationRequestDto registrationRequestDto, string operation)
        {
            var apiRequest = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST,
                $"{StaticDetails.AuthAPIBase}/api/auth/{operation}", registrationRequestDto);

            if (apiRequest is null) return null;

            var request = await _baseService.SendAsync<ResponseDto>(apiRequest);

            return request;
        }

        private async Task<ResponseDto?> SendRequest(LoginRequestDto loginRequestDto, string operation)
        {
            var apiRequest = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST,
                $"{StaticDetails.AuthAPIBase}/api/auth/{operation}", loginRequestDto);

            if (apiRequest is null) return null;

            var request = await _baseService.SendAsync<ResponseDto>(apiRequest);

            return request;
        }
    }
}
