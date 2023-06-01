using Microservices.Web.Models;
using Microservices.Web.Models.Factories;
using Microservices.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
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
                .GetAllEntitiesAsync<ResponseDto>();

                if (response is not null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<CouponDto>>(response.Result?.ToString()!)!;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return View(list);
        }

        public async Task<IActionResult> CouponCreate()
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto couponDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var response = await _couponService.AddEntityAsync<ResponseDto, CouponDto>(couponDto);
                    if(response is not null && response.IsSuccess)
                    {
                        return RedirectToAction(nameof(CouponIndex));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return View(couponDto);
        }
    }
}
