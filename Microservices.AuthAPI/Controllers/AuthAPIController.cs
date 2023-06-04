﻿using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : APIBaseController
    {
        private readonly IAuthService _authService;

        public AuthAPIController(Serilog.ILogger logger, IAuthService authService) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            try
            {
                var errorMessage = await _authService.Register(model);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                LogError(ex);

                return BadRequest("An unexpected error happened during the user registration");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            try
            {
                var loginResponse = await _authService.Login(model);

                if(loginResponse is null) return NotFound("user not found");

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);

                return BadRequest("An unexpected error happened during the user login");
            }
        }
    }
}
