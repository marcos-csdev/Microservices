
using Microservices.CouponAPI.Models.Dto;
using Microservices.CouponAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponAPIController : APIBaseController
    {
        private ICouponRepository _couponRepository;
        public CouponAPIController(Serilog.ILogger logger, ICouponRepository couponRepository) : base(logger)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var couponDtos = await _couponRepository.GetCouponsAsync();

                ControllerResponse.Result = couponDtos!;

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Ok(ControllerResponse.Result);
        }

        [HttpGet("{couponId}")]
        public async Task<IActionResult> GetCouponById(int couponId)
        {
            if (couponId == 0) return BadRequest();

            try
            {
                var couponDto = await _couponRepository.GetCouponByIdAsync(couponId);

                ControllerResponse.Result = couponDto;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Ok(ControllerResponse.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);

                ControllerResponse.Result = newCoupon;

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Created(nameof(Create), ControllerResponse.Result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);

                ControllerResponse.Result = newCoupon;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse.Result);
        }

        [HttpDelete("{couponId}")]
        public async Task<IActionResult> Remove(int couponId)
        {
            if (couponId == 0) return BadRequest();

            try
            {
                var hasBeenDeleted = await _couponRepository.DeleteCouponAsync(couponId);

                ControllerResponse.Result = hasBeenDeleted;

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse.Result);
        }
    }

}
