using Microservices.Web.Models;
using Microservices.Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Microservices.Web.Controllers
{
    [Route("api/[controller]")]
    public class CouponController : BaseController
    {
        private readonly ICouponService _couponService;

        public CouponController(Serilog.ILogger logger, ICouponService couponService) : base(logger)
        {
            _couponService = couponService;
        }

        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list;

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

            return View();
        }
    }
}
