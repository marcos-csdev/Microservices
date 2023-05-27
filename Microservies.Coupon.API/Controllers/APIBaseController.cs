
using Microservices.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.CouponAPI.Controllers
{
    public class APIBaseController : ControllerBase
    {

        protected ResponseDto ControllerResponse;
        protected Serilog.ILogger Logger;

        public APIBaseController(Serilog.ILogger logger)
        {
            ControllerResponse = new ResponseDto();
            Logger = logger;
        }

        protected void LogError(Exception ex)
        {
            ControllerResponse.IsSuccess = false;
            ControllerResponse.ErrorMessages = new List<string> { ex.Message };
            Logger.Error(ex.Message, ex);
        }
    }
}
