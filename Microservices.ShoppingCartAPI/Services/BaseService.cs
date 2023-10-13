using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;
using System.Net.Http;
using Microservices.ShoppingCartAPI.Models.Factories;

namespace Microservices.ShoppingCartAPI.Services
{
    public class BaseService
    {
        private readonly HttpClient _httpClient;

        public BaseService(IHttpClientFactory clientFactory, string name)
        {
            _httpClient = clientFactory.CreateClient(name);
        }
        public BaseService(IHttpContextAccessor contextAccessor)
        {
            _httpClient = HttpClientFactoryFactory.Create(contextAccessor);
        }

        protected async Task<ResponseDto?> SendMessageAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var apiContent = await response.Content.ReadAsStringAsync();
            var deserializedJson = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return deserializedJson;
        }

        protected List<TEntity> DeserializeResponseToList<TEntity>(ResponseDto? response)
        {
            List<TEntity>? list = null;


            if (response == null)
                throw new Exception("Could not retrieve products from the server");

            if (response.IsSuccess)
            {

                var jsonResponse = JArray.Parse(response.Result?.ToString()!);

                list = JsonConvert.DeserializeObject<List<TEntity>>(jsonResponse.ToString());

                if (list == null)
                    throw new Exception("Problem converting list to JSON");
            }
            
            list ??= new List<TEntity>();
            return list;
        }

        protected TEntity DeserializeResponseToEntity<TEntity>(ResponseDto response) where TEntity : class, new()

        {
            if (response == null)
                throw new Exception("Could not retrieve data from the server");

            TEntity? entity = null!;
            if (response != null && response.IsSuccess)
            {
                var jsonResponse = JObject.Parse(response.Result?.ToString()!);

                if (jsonResponse["result"] != null)
                    entity = JsonConvert.DeserializeObject<TEntity>(jsonResponse["result"]!.ToString());
                else
                    entity = JsonConvert.DeserializeObject<TEntity>(jsonResponse.ToString());

                if (entity == null)
                    throw new Exception("Problem converting list to JSON");
            }
            

            entity ??= new TEntity();
            return entity;
        }
    }
}
