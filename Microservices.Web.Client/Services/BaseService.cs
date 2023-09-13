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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            ResponseDto = new ResponseDto();
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }


        public async Task<ResponseDto?> SendAsync(RequestDto apiRequest, bool withBearer = true)
        {
            string apiContent;
            try
            {
                var client = _httpClientFactory.CreateClient("MicroServicesAPI");

                var appType = "application/json";

                var message = SetRequestMessage(apiRequest, appType, withBearer);

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


        private void SetToken(bool withBearer, HttpRequestMessage message)
        {
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();

                message.Headers.Add("Authorization", $"Bearer {token}");
            }
        }

        /// <summary>
        /// Creates an HTTP message with header, URI, HTTP Verb and Body
        /// </summary>
        private HttpRequestMessage SetRequestMessage(RequestDto apiRequest, string appType, bool withBearer)
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", appType);

            SetToken(withBearer, message);

            message.RequestUri = new Uri(apiRequest.Url);

            SetRequestHttpVerb(apiRequest, message);

            //request will have a body on create/update operations
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
