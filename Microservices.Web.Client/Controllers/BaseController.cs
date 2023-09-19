
using Microservices.Web.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

        protected List<TEntity> DeserializeResponseToList<TEntity>(ResponseDto response)
        {
            List<TEntity>? list = null;

            try
            {

                if (response == null)
                    throw new Exception("Could not retrieve products from the server");

                if (response != null && response.IsSuccess)
                {
                    var jsonResponse = JObject.Parse(response.Result?.ToString()!);

                    list = JsonConvert.DeserializeObject<List<TEntity>>(jsonResponse["result"]!.ToString());

                    if (list == null)
                        throw new Exception("Problem converting list to JSON");
                }
                else
                {
                    if (response == null)
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

        protected TEntity DeserializeResponseToEntity<TEntity>(ResponseDto response) where TEntity : class, new()

        {
            TEntity? entity = null;


            if (response == null)
                throw new Exception("Could not retrieve products from the server");

            if (response != null && response.IsSuccess)
            {
                var jsonResponse = JObject.Parse(response.Result?.ToString()!);

                entity = JsonConvert.DeserializeObject<TEntity>(jsonResponse["result"]!.ToString());

                if (entity == null)
                    TempData["error"] = "Problem converting list to JSON";
            }
            else
            {
                if (response == null)
                {
                    TempData["error"] = "Could not retrieve a response from the server";
                }
                else
                {
                    TempData["error"] = response?.DisplayMessage;
                }
            }

            entity ??= new TEntity();
            return entity;
        }


        protected IActionResult SetReturnMessage(ResponseDto response, string message, string actionName, string controllerName)
        {
            if (response == null || response?.IsSuccess == false)
            {
                if (response == null || string.IsNullOrWhiteSpace(response?.DisplayMessage))
                {
                    return NotFound();
                }
                else
                {
                    throw new Exception(response?.DisplayMessage);
                }
            }
            else
            {
                TempData["success"] = message;
                return RedirectToAction(actionName, controllerName);
            }
        }
    }
}
