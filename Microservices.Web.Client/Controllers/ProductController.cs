using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Microservices.Web.Client.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;

        public ProductController(Serilog.ILogger logger, IProductService productService) : base(logger)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> ProductIndex()
        {

            List<ProductDto> products = null!;
            try
            {
                var response = await _productService.GetAllProductsAsync();

                if (response is null) 
                    throw new Exception("Could not retrieve products from the server");

                products = EntityIndex<ProductDto>(response);

                if (products is null)
                    throw new Exception("Problem converting list to JSON");

            }
            catch (Exception ex)
            {
                LogError(ex);
                TempData["error"] = ControllerResponse.ErrorMessages[0];
            }

            return View(products);
        }

        [HttpGet]
        public ActionResult ProductCreate() { return View(); }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var response = await _productService.AddProductAsync(productDto);
                    if (response is not null && response.IsSuccess)
                    {
                        TempData["success"] = "Product created";
                        return RedirectToAction(nameof(ProductIndex));
                    }
                    else
                    {
                        TempData["error"] = response?.DisplayMessage;
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return View(productDto);
        }

        public async Task<IActionResult> ProductRemove(int productId)
        {
            try
            {
                var response = await _productService.RemoveProductAsync(productId);

                if (response == null || response?.IsSuccess == false)
                {
                    if (string.IsNullOrWhiteSpace(response?.DisplayMessage))
                    {
                        TempData["error"] = "Could not retrieve response from API";
                    }
                    else
                    {
                        TempData["error"] = response?.DisplayMessage;
                    }
                }
                else
                {
                    TempData["success"] = "Product removed";
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return RedirectToAction(nameof(ProductIndex));
        }


    }
}
