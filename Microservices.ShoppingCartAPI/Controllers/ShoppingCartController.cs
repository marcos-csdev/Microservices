using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Models.Factories;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Services;
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
        private readonly IProductService _productService;

        public ShoppingCartController(Serilog.ILogger logger, IShoppingCartRepository cartsRepository, IProductService productService) : base(logger)
        {
            _cartRepository = cartsRepository;
            _productService = productService;
        }

        private async Task CalculateTotal(CartDto cartDto)
        {
            var productsDto = await _productService.GetProducts();

            foreach (var item in cartDto.CartDetails!)
            {
                item.productDto = productsDto
                .FirstOrDefault(prod => prod.Id == item.ProductId);

                cartDto.CartHeader!.CartTotal += item.Count * item.productDto!.Price;
            }

        }

        [HttpGet("GetCart/{userId}")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId)) return BadRequest(string.Empty);

                var cartDto = await _cartRepository.GetCartAsync(userId);

                if (cartDto == null || 
                    cartDto.CartDetails == null || 
                    cartDto.CartDetails.Count() == 0 || 
                    cartDto.CartHeader == null) return NotFound();

                await CalculateTotal(cartDto);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, cartDto, "Success");

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
                if (cartDetailsId < 1)
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
