using IdentityModel;
using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace Microservices.Web.Client.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;

        public HomeController(Serilog.ILogger logger, IProductService productService, ICartService cartService) : base(logger)
        {
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDto>? products = null!;
            try
            {
                var response = await _productService.GetAllProductsAsync();

                products = DeserializeResponseToList<ProductDto>(response!);

            }
            catch (Exception ex)
            {
                LogError(ex);
                TempData["error"] = ControllerResponse.ErrorMessages[0];
            }

            return View(products);
        }

        [Authorize]
        [HttpGet("ProductDetails")]
        public async Task<IActionResult> ProductDetails(int productId)
        {
            ShoppingCartDto? cart = null!;
            try
            {
                var respose = await _productService.GetProductByIdAsync(productId);

                cart = DeserializeResponseToEntity<ShoppingCartDto>(respose!);
            }
            catch (Exception ex)
            {
                LogError(ex);
                TempData["error"] = ControllerResponse.ErrorMessages[0];
            }
            return View(cart);
        }

        private CartDto CreateCartDto(ProductDto productDto)
        {
            var userId = User.Claims
                    .FirstOrDefault(usr => usr.Type == JwtClaimTypes.Subject)?.Value;
            var cartHeader = CartHeaderDtoFactory.Create(userId!);

            var cartDetails = CartDetailsDtoFactory.Create(productDto.Id, productDto.Count);

            var listCarts = new List<CartDetailsDto> { cartDetails };

            var cartDto = CartDtoFactory.Create(cartHeader, listCarts);

            return cartDto;
        }

        [Authorize]
        [HttpPost("ProductDetails")]
        public async Task<IActionResult> ProductDetails(ProductDto productDto)
        {
            ResponseDto? response = null!;
            try
            {
                var cartDto = CreateCartDto(productDto);

                response = await _cartService.UpsertCartAsync(cartDto);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Item added to cart";
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            if (response != null)
                TempData["error"] = response.DisplayMessage;

            return View(productDto);

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}