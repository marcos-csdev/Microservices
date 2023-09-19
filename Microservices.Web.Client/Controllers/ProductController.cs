using Microservices.Web.Client.Models;
using Microservices.Web.Client.Services;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

                products = DeserializeResponseToList<ProductDto>(response!);

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
                    if (response != null && response.IsSuccess)
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

                return SetReturnMessage(response!, "Product removed", "Index", "Product");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return RedirectToAction(nameof(ProductIndex));
        }

        [HttpGet]
        public async Task<IActionResult> ProductEdit(int productId)
        {
            var response = await _productService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                var productJson = DeserializeResponseToEntity<ProductDto>(response!);

                return View(productJson);
            }
            else
            {
                TempData["error"] = response?.DisplayMessage;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductEdit([FromBody] ProductDto product)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (product == null) return BadRequest(ModelState);

                    var response = await _productService.UpdateProductAsync(product);

                    var deserializedProduct = DeserializeResponseToEntity<ProductDto>(response!);

                    if(deserializedProduct != null && deserializedProduct.Id > 0)
                        return RedirectToAction(nameof(ProductIndex));

                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }


            return NotFound();
        }


    }
}
