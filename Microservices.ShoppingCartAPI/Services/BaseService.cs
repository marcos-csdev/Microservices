using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microservices.ShoppingCartAPI.Utility;

namespace Microservices.ShoppingCartAPI.Services
{
    public class BaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected async Task<ResponseDto?> SendMessageAsync(string clientName, string url)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            var response = await client.GetAsync(url);
            var apiContent = await response.Content.ReadAsStringAsync();
            var deserializedJson = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            return deserializedJson;
        }
        protected List<TEntity> DeserializeResponseToList<TEntity>(ResponseDto? response)
        {
            List<TEntity>? list = null;


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
                    throw new Exception("Could not retrieve response from the server");
                }
                else
                {
                    throw new Exception(response?.DisplayMessage);
                }
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

            entity ??= new TEntity();
            return entity;
        }
    }
}
