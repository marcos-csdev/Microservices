using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Service
{
    public interface IRoleManagerService
    {
        Task<IdentityResult> CreateAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
    }
}