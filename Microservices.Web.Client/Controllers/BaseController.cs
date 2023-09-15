
using Microservices.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        protected List<TEntity> EntityIndex<TEntity>(ResponseDto response)
        {
            List<TEntity>? list = null;

            try
            {

                if (response is not null && response.IsSuccess)
                {
                    var jsonResponse = JObject.Parse(response.Result?.ToString()!);

                    list = JsonConvert.DeserializeObject<List<TEntity>>(jsonResponse["result"]!.ToString());
                }
                else
                {
                    if (response is null)
                    {
                        throw new Exception("Could not retrieve a response from the server");
                    }
                    else
                    {
                        throw new Exception(response?.DisplayMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            list ??= new List<TEntity>();
            return list;
        }
    }
}
