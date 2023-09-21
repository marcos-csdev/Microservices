using Microservices.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ShoppingCartAPI.Controllers
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
            var message = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
            ControllerResponse.ErrorMessages = new List<string> { message };
            Logger.Error(ex.Message, ex);
        }
    }
}
