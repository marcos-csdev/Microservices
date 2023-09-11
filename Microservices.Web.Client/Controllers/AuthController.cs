using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace Microservices.Web.Client.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, Serilog.ILogger logger, ITokenProvider tokenProvider) : base(logger)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
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
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(content);

                        if (loginResponse is null)
                            throw new Exception("There was a problem with the user token");

                        _tokenProvider.SetToken(loginResponse.Token);

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

        private void AddToClaimsIdentity(string claimProperty, JwtSecurityToken token, ClaimsIdentity identity)
        {
            var claimName = token.Claims.FirstOrDefault(t => t.Type == claimProperty);

            if (claimName is not null)
            {
                identity.AddClaim(
                    new Claim(claimProperty, claimName.Value));
            }
        }

        private async Task SignInUser(LoginResponseDto loginRequest)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(loginRequest.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            //var claimName = JwtRegisteredClaimNames.Name is not null ? JwtRegisteredClaimNames.Name :

            if(token is not null && token.Claims is not null)
            {
                var claimName = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Name);

                if (claimName is not null)
                {
                    identity.AddClaim(
                        new Claim(JwtRegisteredClaimNames.Name, claimName.Value));
                }

                var claimSub = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Sub);
                if (claimSub is not null)
                {
                    identity.AddClaim(
                        new Claim(JwtRegisteredClaimNames.Sub, 
                        claimSub.Value));
                }

                var claimEmail = token.Claims.FirstOrDefault(t => t.Type != JwtRegisteredClaimNames.Email);
                if (claimEmail is not null)
                {
                    identity.AddClaim(
                        new Claim(JwtRegisteredClaimNames.Email, claimEmail.Value));
                }

                var registeredNames = token.Claims.FirstOrDefault(t => t.Type == JwtRegisteredClaimNames.Name);
                if (registeredNames is not null)
                {
                    identity.AddClaim(
                        new Claim(ClaimTypes.Name, registeredNames.Value));
                }

                var registeredEmails = token.Claims.FirstOrDefault(t => t.Value == JwtRegisteredClaimNames.Email);
                if (registeredEmails is not null)
                {
                    identity.AddClaim(
                        new Claim(ClaimTypes.Email, registeredEmails.Value));
                }


            }


            var claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
    }
}
