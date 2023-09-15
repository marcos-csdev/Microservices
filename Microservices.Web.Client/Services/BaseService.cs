using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class BaseService
    {
        private readonly IMessageService _messageService;

        public BaseService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task<ResponseDto?> GetAllEntitiesAsync(string url)
        {
            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET,
                url);

            var response = await _messageService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> GetEntityByIdAsync(int id, string url)
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET,
                url);

            var response = await _messageService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> AddEntityAsync<TEntity>(TEntity? entityDto, string url) where TEntity : class
        {
            if (entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST,
                url,
                entityDto);

            //SendAsync may return null
            var response = await _messageService.SendAsync(request);

            return response;
        }
        public async Task<ResponseDto?> RemoveEntityAsync(int id, string url)
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.DELETE,
                url);


            var response = await _messageService.SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> UpdateEntityAsync<TEntity>(string id, TEntity? entityDto, string url)
            where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(id) || entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.PUT,
                url,
                entityDto);

            var response = await _messageService.SendAsync(request);

            return response;
        }

    }
}
