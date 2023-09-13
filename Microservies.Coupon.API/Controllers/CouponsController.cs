
using AutoMapper;
using Microservices.CouponAPI.Models.Dto;
using Microservices.CouponAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CouponAPI.Controllers
{
    [Route("api/coupons")]
    [ApiController]
    [Authorize]
    public class CouponAPIController : APIBaseController
    {
        private readonly ICouponRepository _couponRepository;

        public CouponAPIController(Serilog.ILogger logger, ICouponRepository couponRepository) : base(logger)
        {
            _couponRepository = couponRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                ControllerResponse = new ResponseDto();

                var couponDtos = await _couponRepository.GetCouponsAsync();
                ControllerResponse = new ResponseDto(true, couponDtos, "Success");

            }
            catch (Exception ex)

            {
                LogError(ex);
            }

            return Ok(ControllerResponse);
        }

        [HttpGet("GetById/{couponId}")]
        public async Task<IActionResult> GetCouponById(int couponId)
        {
            if (couponId == 0) return BadRequest();

            ControllerResponse = new ResponseDto();
            try
            {
                var couponDto = await _couponRepository.GetCouponByIdAsync(couponId);

                if (couponDto is null) return NotFound("The provided coupon was not found");

                ControllerResponse = new ResponseDto(true, couponDto, "Success");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Ok(ControllerResponse);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);

                ControllerResponse = new ResponseDto(true, newCoupon.ToString(), "Success");

                return Created(nameof(Create), ControllerResponse);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem(ControllerResponse.DisplayMessage, nameof(Create));
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            ControllerResponse = new ResponseDto();
            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);
                ControllerResponse = new ResponseDto(true, couponDto, "Success");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse);
        }

        [HttpDelete]
        [Route("Remove/{couponId}")]
        public async Task<IActionResult> Remove(int couponId)
        {
            if (couponId == 0) return BadRequest();

            ControllerResponse = new ResponseDto();
            try
            {
                var hasBeenDeleted = await _couponRepository.DeleteCouponAsync(couponId);
                ControllerResponse = new ResponseDto(true, hasBeenDeleted, "Success");

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse);
        }
    }

}
