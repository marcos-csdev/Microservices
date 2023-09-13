
using AutoMapper;
using Microservices.CouponAPI;
using Microservices.CouponAPI.Models.Dto;
using Microservices.CouponAPI.Models.Factories;
using Microservices.CouponAPI.Repositories;
using Microservices.CouponAPI.Utility;
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
            ControllerResponse = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var couponDtos = await _couponRepository.GetCouponsAsync();
                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, couponDtos, "Success");

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

            try
            {
                var couponDto = await _couponRepository.GetCouponByIdAsync(couponId);

                if (couponDto is null) return NotFound("The provided coupon was not found");

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, couponDto, "Success");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Ok(ControllerResponse);
        }

        [HttpPost]
        [Route("Create")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Create([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);

                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, newCoupon.ToString(), "Success");

                return Created(nameof(Create), ControllerResponse);

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Problem("An error happened during the coupon creation", nameof(Create));
        }

        [HttpPut]
        [Route("Update")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Update([FromBody] CouponDto couponDto)
        {
            if (couponDto == null) return BadRequest();

            try
            {
                var newCoupon = await _couponRepository.UpsertCouponAsync(couponDto);
                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, couponDto, "Success");
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse);
        }

        [HttpDelete]
        [Route("Remove/{couponId}")]
        [Authorize(Roles = StaticDetails.RoleAdmin)]
        public async Task<IActionResult> Remove(int couponId)
        {
            if (couponId == 0) return BadRequest();

            try
            {
                var hasBeenDeleted = await _couponRepository.DeleteCouponAsync(couponId);
                ControllerResponse = ResponseDtoFactory.CreateResponseDto(true, hasBeenDeleted, "Success");

            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return Accepted(ControllerResponse);
        }
    }

}
