using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Service
{
    public class UserManagerService : IUserManagerService
    {

        private readonly UserManager<MSUser> _userManager;

        public UserManagerService(UserManager<MSUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckPasswordAsync(MSUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> CreateAsync(MSUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(MSUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IList<string>> GetRolesAsync(MSUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
