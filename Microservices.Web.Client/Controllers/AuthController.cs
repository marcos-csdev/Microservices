using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Web.Client.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, Serilog.ILogger logger) : base(logger)
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
        }
        [HttpGet]
        public IActionResult Logout()
        {

            return View();
        }
    }
}
