using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        private List<SelectListItem> PopulateSelectList()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem() {
                    Text = StaticDetails.RoleAdmin,
                    Value = StaticDetails.RoleAdmin
                },
                new SelectListItem() {
                    Text = StaticDetails.RoleCustomer,
                    Value = StaticDetails.RoleCustomer
                },
            };

            return roleList;
        }

        [HttpGet]
        public IActionResult Register()
        {
            

            ViewBag.RoleList = PopulateSelectList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto registrationRequest)
        {

            if(ModelState.IsValid)
            {
                var responseDto = await _authService.RegisterAsync(registrationRequest);
                //if role has not been defined, assume its a customer
                if (responseDto is not null && responseDto.IsSuccess) 
                {
                    if (string.IsNullOrWhiteSpace(responseDto.Role))
                    {
                        responseDto.Role = StaticDetails.RoleCustomer;
                    }
                    else
                    {
                        var assignRole = await _authService.AssignRoleAsync(registrationRequest);

                        if (assignRole is not null && assignRole.IsSuccess)
                        {
                            TempData["success"] = "Registration successful";
                            return RedirectToAction(nameof(Login));
                        }
                    }
                }
            }

            ViewBag.RoleList = PopulateSelectList();

            return View(registrationRequest);
        }
        [HttpGet]
        public IActionResult Logout()
        {

            return View();
        }
    }
}
