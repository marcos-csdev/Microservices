﻿using Microservices.Web.Client.Models;
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
            List<CouponDto>? coupons = null;

            try
            {
                var response = await _couponService
                .GetAllCouponsAsync();


                coupons = DeserializeResponseToList<CouponDto>(response!);

            }
            catch (Exception ex)
            {
                LogError(ex);
                TempData["error"] = ControllerResponse.ErrorMessages[0];
            }

            return View(coupons);
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

                    var response = await _couponService.AddCouponAsync(couponDto);
                    if (response != null && response.IsSuccess)
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
                var response = await _couponService.RemoveCouponAsync(couponId);
               
                SetReturnMessage(response!, "Coupon removed", "Index", "Coupon");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return RedirectToAction(nameof(CouponIndex));
        }



    }
}
