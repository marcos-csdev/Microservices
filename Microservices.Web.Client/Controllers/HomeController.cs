using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Microservices.Web.Client.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IProductService _productService;

        public HomeController(Serilog.ILogger logger, IProductService productService) : base(logger)
        {
            _productService = productService;
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