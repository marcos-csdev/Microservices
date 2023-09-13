using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class CouponService : BaseService, ICouponService
    {

        public CouponService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) : base(httpClientFactory, tokenProvider)
        {
        }

        public async Task<ResponseDto?> AddEntityAsync<TEntity>(TEntity? entityDto) where TEntity : class
        {
            if (entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/create", 
                entityDto);

            //SendAsync may return null
            var response = await SendAsync(request);

            return response;
        }


        public async Task<ResponseDto?> GetAllEntitiesAsync() 
        {
            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET, 
                $"{StaticDetails.CouponAPIBase}/api/coupons");
           
            var response = await SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> GetEntityByIdAsync(int id) 
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/GetById/{id}");

            var response = await SendAsync(request);

            return response;
        }


        public async Task<ResponseDto?> RemoveEntityAsync(int id)
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto         (StaticDetails.ApiType.DELETE, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/remove/{id}");
                

            var response = await SendAsync(request);

            return response;
        }

        public async Task<ResponseDto?> UpdateEntityAsync<TEntity>(string id, TEntity? entityDto) 
            where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(id) || entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.PUT, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/update", 
                entityDto);
            
            var response = await SendAsync(request);

            return response;
        }

    }
}
