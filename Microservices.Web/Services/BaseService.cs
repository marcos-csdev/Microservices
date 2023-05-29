using Microservices.Web.Models;
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
                var message = SetRequestMessage(apiRequest, appType, client);

                var apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var deserializedResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return deserializedResponse;
            }
            catch(ArgumentNullException ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "API reponse could not be deserialized into JSON",
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };
                var response = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(response);

                return apiResponseDto;
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { ex.Message },
                    IsSuccess = false
                };

                var response = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(response);

                return apiResponseDto;
            }
        }

        private static HttpRequestMessage SetRequestMessage(RequestDto apiRequest, string appType, HttpClient client)
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", appType);
            message.RequestUri = new Uri(apiRequest.Url);

            client.DefaultRequestHeaders.Clear();

            SetApiRequestMethod(apiRequest, message);

            if (apiRequest.Data is not null)
            {
                var serializedJson = JsonConvert.SerializeObject(apiRequest);
                message.Content = new StringContent(serializedJson, Encoding.UTF8, appType);
            }

            return message;
        }

        private static void SetApiRequestMethod(RequestDto apiRequest, HttpRequestMessage message)
        {
            switch (apiRequest.ApiType)
            {
                case StaticDetails.ApiType.POST:
                    message.Method = HttpMethod.Post;
                    break;
                case StaticDetails.ApiType.PUT:
                    message.Method = HttpMethod.Put;
                    break;
                case StaticDetails.ApiType.DELETE:
                    message.Method = HttpMethod.Delete;
                    break;
                default:
                    message.Method = HttpMethod.Get;
                    break;
            }
        }

        

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }

    }
}
