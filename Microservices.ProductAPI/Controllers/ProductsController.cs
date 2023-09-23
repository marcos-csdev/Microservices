using Microservices.ProductAPI.Models.Dto;
using Microservices.ProductAPI.Models.Factories;
using Microservices.ProductAPI.Repositories;
using Microservices.ProductAPI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ProductAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : APIBaseController
    {
        private readonly IProductRepository _productsRepository;

        public ProductsController(Serilog.ILogger logger, IProductRepository productsRepository):base(logger)
        {
            _productsRepository = productsRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var products = await _productsRepository.GetAllProductsAsync();

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, products, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened retrieving the products");
        }
        
        [HttpGet("GetById/{productId}")]
        public async Task<IActionResult> GetProductByIDAsync(int productId)
        {
            try
            {
                var product = await _productsRepository.GetProductByIdAsync(productId);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, product, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened retrieving the product");
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Create(ProductDto product)
        {
            if (product == null) return BadRequest();

            try
            {
                var createdProduct = await _productsRepository.UpsertProductAsync(product);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, createdProduct, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened creating the product");
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Update([FromBody]ProductDto product)
        {
            if (product == null) return BadRequest();

            try
            {


                var isProductUpdated = await _productsRepository.UpsertProductAsync(product);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, product, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return Problem("An error happened updating the product");
        }

        [HttpDelete]
        [Route("Remove/{productId}")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Remove(int productId)
        {
            if(productId <= 0) return BadRequest();

            try
            {
                var isProductDeleted = await _productsRepository.DelectProductAsync(productId);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, productId, "Success");

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened deleting the product");

        }
    }
}
