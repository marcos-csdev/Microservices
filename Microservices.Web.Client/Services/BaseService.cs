using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;
using Newtonsoft.Json;
using NuGet.Protocol;
using System.Data.SqlTypes;
using System.Net;
using System.Text;

namespace Microservices.Web.Client.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto ResponseDto { get; set; } = null!;
        public IHttpClientFactory HttpClientFactory { get; set; } = null!;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
            ResponseDto = new ResponseDto();
        }

        public async Task<ResponseDto?> SendAsync(RequestDto apiRequest)
        {
            string apiContent;
            try
            {
                var client = HttpClientFactory.CreateClient("MSCouponAPI");

                var appType = "application/json";

                var message = SetRequestMessage(apiRequest, appType);

                var apiResponse = await client.SendAsync(message);
                apiContent = await apiResponse.Content.ReadAsStringAsync();

                if(apiResponse.StatusCode != HttpStatusCode.OK)
                {
                    return new ResponseDto(false, null!, apiContent);
                }

                return new ResponseDto(true, apiContent, "Success");
            }
            
            catch (Exception ex)
            {
                return new ResponseDto(false, null!, ex.Message);
            }
        }

        

        private static HttpRequestMessage SetRequestMessage(RequestDto apiRequest, string appType)
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", appType);
            message.RequestUri = new Uri(apiRequest.Url);

            SetRequestHttpVerb(apiRequest, message);

            //request will have a body on create/update
            if (apiRequest.Body is not null)
            {
                var serializedJson = JsonConvert.SerializeObject(apiRequest.Body);
                message.Content = new StringContent(serializedJson, Encoding.UTF8, appType);
            }

            return message;
        }

        private static void SetRequestHttpVerb(RequestDto apiRequest, HttpRequestMessage message)
        {
            _ = apiRequest.ApiType switch
            {
                StaticDetails.ApiType.POST =>
                    message.Method = HttpMethod.Post,

                StaticDetails.ApiType.PUT =>
                    message.Method = HttpMethod.Put,

                StaticDetails.ApiType.DELETE =>
                    message.Method = HttpMethod.Delete,

                _ =>
                    message.Method = HttpMethod.Get,
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }
}
