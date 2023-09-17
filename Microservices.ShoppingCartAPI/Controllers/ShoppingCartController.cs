using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Models.Factories;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ShoppingCartAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : APIBaseController
    {
        private readonly IShoppingCartRepository _productsRepository;

        public ShoppingCartController(Serilog.ILogger logger, IShoppingCartRepository productsRepository):base(logger)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cart = await _productsRepository.GetCartAsync();

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, cart, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened retrieving the products");
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
