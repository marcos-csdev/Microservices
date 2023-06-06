using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Service
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> CreateAsync(string roleName)
        {
            return await _roleManager.CreateAsync(new IdentityRole(roleName));
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}
