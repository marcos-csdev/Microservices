
using Microservices.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.Web.Client.Controllers
{
    public abstract class BaseController : Controller
    {

        protected ResponseDto ControllerResponse;
        protected Serilog.ILogger Logger;

        public BaseController(Serilog.ILogger logger)
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
