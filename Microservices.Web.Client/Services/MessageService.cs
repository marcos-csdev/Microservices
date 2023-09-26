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
    public class MessageService : IMessageService
    {
        public ResponseDto ResponseDto { get; set; } = null!;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public MessageService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            ResponseDto = new ResponseDto();
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }


        public async Task<ResponseDto?> SendAsync(RequestDto apiRequest, bool withBearer = true, string clientName = "MicroServicesClient")
        {
            string apiContent;
            try
            {
                var client = _httpClientFactory.CreateClient(clientName);

                var appType = StaticDetails.AppType;

                var message = SetRequestMessage(apiRequest, appType, withBearer);

                var apiResponse = await client.SendAsync(message);
                apiContent = await apiResponse.Content.ReadAsStringAsync();

                var acceptableResults = new HttpStatusCode[] { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted, HttpStatusCode.NoContent};

                //if response is not "OK", "Accepted" or "Created"
                if(!acceptableResults.Contains(apiResponse.StatusCode))
                {
                    var errorMessage = !string.IsNullOrWhiteSpace(apiContent)? apiContent : apiResponse.ReasonPhrase;

                    return ResponseDtoFactory.CreateResponseDto(false, null!, errorMessage!);
                }

                return ResponseDtoFactory.CreateResponseDto(true, apiContent, "Success");
            }
            
            catch (Exception ex)
            {
                return ResponseDtoFactory.CreateResponseDto(false, null!, ex.Message);
            }
        }


        private void SetMessageToken(bool withBearer, HttpRequestMessage message)
        {
            if (withBearer)
            {
                var token = _tokenProvider.GetToken();

                message.Headers.Add("Authorization", $"Bearer {token}");
            }
        }

        private static void SetMessageBody(RequestDto apiRequest, string appType, HttpRequestMessage message)
        {
            if (apiRequest.Body != null)
            {
                var serializedJson = JsonConvert.SerializeObject(apiRequest.Body);
                message.Content = new StringContent(serializedJson, Encoding.UTF8, appType);
            }
        }

        /// <summary>
        /// Creates an HTTP message with header, URI, HTTP Verb and Body
        /// </summary>
        private HttpRequestMessage SetRequestMessage(RequestDto apiRequest, string appType, bool withBearer)
        {
            var message = new HttpRequestMessage();
            message.Headers.Add("Accept", appType);
            message.RequestUri = new Uri(apiRequest.Url);

            SetMessageToken(withBearer, message);

            SetMessageHttpVerb(apiRequest.ApiType, message);

            //request will have a body on create/update operations
            SetMessageBody(apiRequest, appType, message);

            return message;
        }


        private static void SetMessageHttpVerb(StaticDetails.ApiType apiType, HttpRequestMessage message)
        {
            _ = apiType switch
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
