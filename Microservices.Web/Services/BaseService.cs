using Microservices.Web.Models;
using Microservices.Web.Models.Factories;
using Microservices.Web.Services.Abstractions;
using Microservices.Web.Utility;
using Newtonsoft.Json;
using System.Data.SqlTypes;
using System.Text;

namespace Microservices.Web.Services
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

        public async Task<T?> SendAsync<T>(RequestDto apiRequest)
        {
            try
            {
                var client = HttpClientFactory.CreateClient("MSCouponAPI");

                var appType = "application/json";

                var message = SetRequestMessage(apiRequest, appType);

                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return deserializedResponse;
            }
            catch(ArgumentNullException ex)
            {
                var responseDto = ResponseDtoFactory.CreateResponseDto(
                    "API reponse could not be deserialized into JSON", 
                    new List<string> { ex.Message }, 
                    false);
                
                var response = JsonConvert.SerializeObject(responseDto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(response);

                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var responseDto = ResponseDtoFactory.CreateResponseDto(
                    "Error", 
                    new List<string> { ex.Message }, 
                    false);
                
                var response = JsonConvert.SerializeObject(responseDto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(response);

                return apiResponseDto;
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
