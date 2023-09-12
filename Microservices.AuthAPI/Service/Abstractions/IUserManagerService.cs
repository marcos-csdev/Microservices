using Microservices.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Service.Abstractions
{
    public interface IUserManagerService
    {
        Task<IdentityResult> AddToRoleAsync(MSUser user, string roleName);
        Task<bool> CheckPasswordAsync(MSUser user, string password);
        Task<IdentityResult> CreateAsync(MSUser user, string password);
        Task<IList<string>> GetRolesAsync(MSUser user);
    }
}