using Microservices.EmailAPI.Models.Dto;

namespace Microservices.EmailAPI.Services
{
    public interface IEmailService
    {
        Task CreateEmail(CartDto cartDto);
        Task RegisterUserEmail(string email);
    }
}