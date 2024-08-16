using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Service.Abstractions;
using Microservices.MessageBus;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : APIBaseController
    {
        private const string _registerQueue = "TopicAndQueueNames:RegisterUserQueue";

        private readonly IAuthService _authService;
        private readonly IMessageBusProducer _messageBus;
        private readonly string? _queueName;
        private readonly string? _exchangeName;

        public AuthAPIController(Serilog.ILogger logger, IAuthService authService, IMessageBusProducer messageBus, IConfiguration configuration) : base(logger)
        {
            _authService = authService;
            _messageBus = messageBus;
            _queueName = configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");
            _exchangeName = configuration.GetValue<string>("TopicAndQueueNames:ExchangeName");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            try
            {
                var errorMessage = await _authService.Register(model);

                if (!string.IsNullOrWhiteSpace(errorMessage))
                    return BadRequest(errorMessage);

                _messageBus.PublishMessage(model.Email, _queueName!, _exchangeName!);

                return Ok();
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return BadRequest("An unexpected error happened during the user registration");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            try
            {
                var loginResponse = await _authService.Login(model);

                if (loginResponse == null) return NotFound("user not found");

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);

                return BadRequest("An unexpected error happened during the user login");
            }
        }

        [HttpPost("assignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto request)
        {
            try
            {
                var isRoleAssigned = await _authService.AssignRole(request.Email, request.Role);

                if (isRoleAssigned == false) return BadRequest(isRoleAssigned);

                return Ok(isRoleAssigned);
            }
            catch (Exception ex)
            {
                LogError(ex);

                return Problem("An unexpected error happened during the role assignment");
            }
        }
    }
}
