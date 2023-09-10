using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;

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
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            try
            {
                var response = await _authService.LoginAsync(loginRequest);

                if (response is not null && response.IsSuccess)
                {
                    var content = response.Result?.ToString();
                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(content);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var message = response is not null ? response.DisplayMessage : "An unexpected error happened";

                    ModelState.AddModelError("CustomError", message);

                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }


            return View(loginRequest);
        }

        private static List<SelectListItem> PopulateSelectList()
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
            ResponseDto? responseDto = null!;

            try
            {
                if (ModelState.IsValid)
                {
                    responseDto = await _authService.RegisterAsync(registrationRequest);
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

                            if (assignRole is null || assignRole.IsSuccess == false)
                            {
                                ViewBag.RoleList = PopulateSelectList();
                                return View(registrationRequest);
                            }
                        }

                        TempData["success"] = "Registration successful";
                        return RedirectToAction(nameof(Login));
                    }
                }

            }
            catch (Exception ex)
            {
                LogError(ex);
                return View(ControllerResponse);
            }

            TempData["error"] = responseDto is not null && !string.IsNullOrWhiteSpace(responseDto.DisplayMessage) ? responseDto.DisplayMessage : "An unexpected error happened";

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
