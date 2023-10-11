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
        [HttpGet]
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
            TempData["error"] = "It was not possible to acquire the cart";

            return View();
        }

        private async Task<CartDto> LoadCartDto()
        {
            var userId = User.Claims.FirstOrDefault(usr => usr.Type == JwtRegisteredClaimNames.Sub)?.Value;

            if (string.IsNullOrWhiteSpace(userId)) return null!;

            var response = await _cartService.GetCartByIdAsync(userId);

            //empty cart
            if (response == null ||
                response.Result == null ||
                response.Result.ToString() == "") return null!;

            var cartDto = DeserializeResponseToEntity<CartDto>(response!);

            cartDto ??= new CartDto();

            return cartDto;
        }

        public async Task<IActionResult> RemoveCart(int cartDetailsId)
        {
            try
            {
                var response = await _cartService.RemoveCartAsync(cartDetailsId);

                if(response !=  null && response.IsSuccess)
                {
                    TempData["success"] = "Cart updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            TempData["error"] = "There was a problem removing the cart.";
            return View();
        }

        public async Task<IActionResult> RemoveCoupon(string userId)
        {
            try
            {
                var response = await _cartService.RemoveCouponAsync(userId);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Cart updated successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            TempData["error"] = "There was a problem removing the coupon.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCouponCode(CartDto cartDto)
        {
            try
            {
                var response = await _cartService.ApplyCouponCodeAsync(cartDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Cart updated successfully";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            TempData["error"] = "There was a problem applying the coupon.";
            return View(cartDto);
        }
    }
}
