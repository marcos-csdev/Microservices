using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microservices.Web.Client.Controllers
{
    [Authorize]
    public class CouponController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponController(Serilog.ILogger logger, ICouponService couponService) : base(logger)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? list = null;

            try
            {
                var response = await _couponService
                .GetAllEntitiesAsync();

                if (response is not null && response.IsSuccess)
                {
                    var json = JObject.Parse(response.Result?.ToString()!);

                    list = JsonConvert.DeserializeObject<List<CouponDto>>(json["result"]!.ToString());
                }
                else
                {
                    list ??= new List<CouponDto>();
                    TempData["error"] = response?.DisplayMessage;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                TempData["error"] = ControllerResponse.ErrorMessages[0];
            }

            return View(list);
        }

        public IActionResult CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var response = await _couponService.AddEntityAsync(couponDto);
                    if (response is not null && response.IsSuccess)
                    {
                        TempData["success"] = "Coupon created";
                        return RedirectToAction(nameof(CouponIndex));
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

            return View(couponDto);
        }



        public async Task<IActionResult> CouponRemove(int couponId)
        {
            try
            {
                var response = await _couponService.RemoveEntityAsync(couponId);

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
                    TempData["success"] = "Coupon removed";
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return RedirectToAction(nameof(CouponIndex));
        }

        public IActionResult CouponUpdate()
        {
            return View();
        }


    }
}
