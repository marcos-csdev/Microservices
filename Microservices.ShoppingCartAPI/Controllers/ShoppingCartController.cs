using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Models.Factories;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Microservices.ShoppingCartAPI.Controllers
{
    [Route("api/products")]
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

        //[HttpGet("GetById/{productId}")]
        //public async Task<IActionResult> GetProductByIDAsync(int productId)
        //{
        //    try
        //    {
        //        var product = await _productsRepository.GetProductByIdAsync(productId);

        //        ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, product, "Success");

        //        return Ok(ControllerResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }

        //    return Problem("An error happened retrieving the product");
        //}

        //[HttpPost]
        //[Route("Create")]
        //[Authorize(Roles = StaticDetails.RoleAdmin)]
        //public async Task<IActionResult> Create(ProductDto product)
        //{
        //    if (product == null) return BadRequest();

        //    try
        //    {
        //        var createdProduct = await _productsRepository.UpsertProductAsync(product);

        //        ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, createdProduct, "Success");

        //        return Ok(ControllerResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }

        //    return Problem("An error happened creating the product");
        //}

        //[HttpPut]
        //[Route("Update")]
        //[Authorize(Roles = StaticDetails.RoleAdmin)]
        //public async Task<IActionResult> Update([FromBody]ProductDto product)
        //{
        //    if (product == null) return BadRequest();

        //    try
        //    {


        //        var isProductUpdated = await _productsRepository.UpsertProductAsync(product);

        //        ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, product, "Success");

        //        return Ok(ControllerResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }
        //    return Problem("An error happened updating the product");
        //}

        //[HttpDelete]
        //[Route("Remove/{productId}")]
        //[Authorize(Roles = StaticDetails.RoleAdmin)]
        //public async Task<IActionResult> Remove(int productId)
        //{
        //    if(productId <= 0) return BadRequest();

        //    try
        //    {
        //        var isProductDeleted = await _productsRepository.DelectProductAsync(productId);

        //        ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, productId, "Success");

        //        return Ok(ControllerResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogError(ex);
        //    }

        //    return Problem("An error happened deleting the product");

        //}
    }
}
