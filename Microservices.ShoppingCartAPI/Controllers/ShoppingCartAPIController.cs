using Microservices.MessageBus;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Models.Factories;
using Microservices.ShoppingCartAPI.Repositories;
using Microservices.ShoppingCartAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ShoppingCartAPI.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartAPIController : APIBaseController
    {
        private readonly IShoppingCartRepository _cartRepository;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;

        private readonly IRabbitMQMB _rabbitMQMB;

        public ShoppingCartAPIController(Serilog.ILogger logger, IShoppingCartRepository cartsRepository, IProductService productService, ICouponService couponService, IRabbitMQMB rabbitMQMB) : base(logger)
        {
            _cartRepository = cartsRepository;
            _productService = productService;
            _couponService = couponService;
            _rabbitMQMB = rabbitMQMB;
        }

        private async Task CalculateTotal(CartDto cartDto)
        {
            var productsDto = await _productService.GetProducts();

            foreach (var item in cartDto.CartDetails!)
            {
                item.ProductDto = productsDto
                .FirstOrDefault(prod => prod.Id == item.ProductId);

                cartDto.CartHeader!.CartTotal += item.Count * item.ProductDto!.Price;
            }

        }

        private async Task ApplyDiscounts(CartDto cartDto)
        {
            var cartHeader = cartDto.CartHeader;
            if (!string.IsNullOrWhiteSpace(cartHeader!.CouponCode))
            {
                var coupon = await _couponService.GetCoupon("GetByCode", cartHeader.CouponCode);

                if (coupon != null &&
                    cartDto.CartHeader != null &&
                    cartDto.CartHeader.CartTotal > coupon.MinExpense)
                {
                    cartDto.CartHeader.CartTotal -= coupon.MinExpense;
                    cartDto.CartHeader.Discount = coupon.Discount;
                }
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
                    !cartDto.CartDetails.Any() ||
                    cartDto.CartHeader == null)
                {
                    //customer has nothing in the cart, return right away
                    return NoContent();
                }

                await CalculateTotal(cartDto);

                await ApplyDiscounts(cartDto);

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

        [HttpPut("ApplyCouponCode")]
        public async Task<IActionResult> ApplyCouponCode([FromBody] CartDto cartDto)
        {
            try
            {
                if (cartDto == null || cartDto.CartHeader == null)
                {
                    ControllerResponse = ResponseDtoFactory.CreateResponseDto(false, null, "No cart has been acquired");

                    return BadRequest(ControllerResponse);
                }

                await _cartRepository.UpdateCouponCodeAsync(cartDto);

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return Problem("An error happened applying the coupon");
        }

        [HttpDelete("RemoveCoupon/{userId}")]
        public async Task<IActionResult> RemoveCoupon(string userId)
        {
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    ControllerResponse = ResponseDtoFactory.CreateResponseDto(false, null, "No coupon has been acquired");

                    return BadRequest(ControllerResponse);
                }

                await _cartRepository.RemoveCouponAsync(userId);

                return Ok(ControllerResponse);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            return Problem("An error happened applying the coupon");
        }

        [HttpDelete("RemoveCart/{cartDetailsId}")]
        public async Task<IActionResult> RemoveCart(int cartDetailsId)
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

        [HttpPost("EmailCartRequest")]
        public async Task<IActionResult> EmailCartRequest([FromBody] CartDto cartDto)
        {
            try
            {
                await _rabbitMQMB.PublishMessage(cartDto);
                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

    }
}
