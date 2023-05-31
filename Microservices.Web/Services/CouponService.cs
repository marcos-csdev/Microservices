using Microservices.Web.Models;
using Microservices.Web.Models.Factories;
using Microservices.Web.Services.Abstractions;
using Microservices.Web.Utility;

namespace Microservices.Web.Services
{
    public class CouponService : BaseService, ICouponService
    {

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<T?> AddEntityAsync<T>(T entityDto) where T : class
        {
            if (entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST, $"{StaticDetails.CouponAPIBase}/api/coupons", entityDto);

            //SendAsync may return null
            var response = await SendAsync<T>(request);

            return response;
        }


        public async Task<T?> GetAllEntitiesAsync<T>() where T : class
        {
            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET, 
                $"{StaticDetails.CouponAPIBase}api/coupons");
           
            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> GetEntityByIdAsync<T>(int id) where T : class
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.GET, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/{id}");

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> RemoveEntityAsync<T>(int id) where T : class
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto         (StaticDetails.ApiType.DELETE, 
                $"{StaticDetails.CouponAPIBase}/api/coupons/{id}");
                

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> UpdateEntityAsync<T>(string id, T entityDto) where T : class
        {
            if (string.IsNullOrWhiteSpace(id) || entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.PUT, $"{StaticDetails.CouponAPIBase}/api/coupons{id}", entityDto);
            

            var response = await SendAsync<T>(request);

            return response;
        }
    }
}
