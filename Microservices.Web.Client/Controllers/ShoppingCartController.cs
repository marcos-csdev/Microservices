using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Microservices.Web.Client.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly ICartService _cartService;

        public ShoppingCartController(Serilog.ILogger logger, ICartService cartService) : base(logger)
        {
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cartDto = await LoadCartDto();

                return View(cartDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("It was not possible to acquire the cart");
        }

        private async Task<CartDto> LoadCartDto()
        {
            var userId = User.Claims.FirstOrDefault(usr => usr.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return null!;

            var response = await _cartService.GetCartByIdAsync(userId);

            //empty cart
            if(response == null || 
                response.Result == null || 
                response.Result.ToString() == "") return null!;

            var cartDto = DeserializeResponseToEntity<CartDto>(response!);

            cartDto ??= new CartDto();  

            return cartDto;
        }
    }
}
