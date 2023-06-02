using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Models
{
    /// <summary>
    /// columns can be added to the AspNetUsers table here
    /// </summary>
    public class MSUser : IdentityUser
    {
        public string Name { get; set; } = "";
    }
}
