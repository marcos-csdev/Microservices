using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Repositories
{
    public interface IUserRepository
    {
        Task<EntityState> DeleteUserAsync(string userId);
        Task<MSUser?> GetDbUserByEmailAsync(string email);
        Task<MSUser?> GetDbUserByUserNameAsync(string userName);
        Task<MSUserDto> GetUserByIdAsync(string userId);
    }
}