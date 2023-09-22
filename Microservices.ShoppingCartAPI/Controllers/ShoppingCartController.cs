using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Models.Factories;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Microservices.ShoppingCartAPI.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartController : APIBaseController
    {
        private readonly IShoppingCartRepository _cartRepository;

        public ShoppingCartController(Serilog.ILogger logger, IShoppingCartRepository cartsRepository) : base(logger)
        {
            _cartRepository = cartsRepository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId)) return BadRequest(string.Empty);

                var cart = await _cartRepository.GetCartAsync(userId);

                var products =

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, cart, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened retrieving the products");
        }

        

        [HttpPost("Upsert")]
        public async Task<IActionResult> Upsert(CartDto cartDto)
        {
            try
            {
                if (cartDto == null || cartDto.CartHeader == null || cartDto.CartDetails == null)
                {
                    ControllerResponse = ResponseDtoFactory.CreateResponseDto(false, null, "No cart has been acquired");

                    return BadRequest(ControllerResponse);
                }

                await _cartRepository.UpsertCartAsync(cartDto);
                ControllerResponse.Result = cartDto;

                return Ok(ControllerResponse);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened updating the products");
        }

        [HttpDelete("RemoveCart")]
        public async Task<IActionResult> RemoveCart([FromBody] int cartDetailsId)
        {
            try
            {
                if(cartDetailsId < 1)
                {
                    ControllerResponse = ResponseDtoFactory.CreateResponseDto(false, null, "No cart has been acquired");

                    return BadRequest(ControllerResponse);
                }

                await _cartRepository.RemoveCartAsync(cartDetailsId);
                ControllerResponse.Result = cartDetailsId;
                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return Problem("An error happened removing the cart");
        }

    }
}
