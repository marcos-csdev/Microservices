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
            return View(new LoginRequestDto());
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
                        var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(content) ?? throw new Exception("There was a problem with the user token");

                        await SignInUser(loginResponse);

                        _tokenProvider.SetToken(loginResponse.Token);

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    var message = response is not null ? response.DisplayMessage : "An unexpected error happened";

                    //ModelState.AddModelError("CustomError", message);

                    TempData["error"] = message;
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
                        if (string.IsNullOrWhiteSpace(registrationRequest.Role))
                        {
                            registrationRequest.Role = StaticDetails.RoleCustomer;
                        }
                        else
                        {
                            var assignRole = await _authService.AssignRoleAsync(registrationRequest);

                            if (assignRole is null || assignRole.IsSuccess == false)
                            {
                                TempData["error"] = "Could not assign a role to the user";
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
            }

            TempData["error"] = responseDto is not null && !string.IsNullOrWhiteSpace(responseDto.DisplayMessage) ? responseDto.DisplayMessage : "An unexpected error happened";

            ViewBag.RoleList = PopulateSelectList();
            return View(registrationRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private static void AddToClaimsIdentity(string claimProperty, JwtSecurityToken token, ClaimsIdentity identity)
        {
            var claimName = token.Claims.FirstOrDefault(t => t.Type == claimProperty);

            if (claimName is not null)
            {
                identity.AddClaim(
                    new Claim(claimProperty, claimName.Value));
            }
        }
        private static void AddToClaimsIdentity(string claimType, string claimProperty, JwtSecurityToken token, ClaimsIdentity identity)
        {
            var claimName = token.Claims.FirstOrDefault(t => t.Type == claimProperty);

            if (claimName is not null)
            {
                identity.AddClaim(
                    new Claim(claimType, claimName.Value));
            }
        }

        private static void AddClaimsToIdentity(ClaimsIdentity identity, JwtSecurityToken? token)
        {
            if (token is not null && token.Claims is not null)
            {

                AddToClaimsIdentity(JwtRegisteredClaimNames.Name, token, identity);

                AddToClaimsIdentity(JwtRegisteredClaimNames.Sub, token, identity);

                AddToClaimsIdentity(JwtRegisteredClaimNames.Email, token, identity);

                AddToClaimsIdentity(ClaimTypes.Name, JwtRegisteredClaimNames.Email, token, identity);

                //considers the .NET role authentication 
                AddToClaimsIdentity(ClaimTypes.Role, "role", token, identity);

            }
        }

        private async Task SignInUser(LoginResponseDto loginRequest)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(loginRequest.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            AddClaimsToIdentity(identity, token);

            var claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        }
    }
}
