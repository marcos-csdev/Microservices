using Microservices.Web.Models;
using Microservices.Web.Models.Factories;
using Microservices.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpGet]
        public IActionResult Login(string userName, string password)
        {
            var loginRequest = LoginRequestDtoFactory.Create(userName, password);

            return View(loginRequest);
        }
        [HttpGet]
        public IActionResult Register()
        {
            
            return View();
        }[HttpGet]
        public IActionResult Logout()
        {
            
            return View();
        }
    }
}
