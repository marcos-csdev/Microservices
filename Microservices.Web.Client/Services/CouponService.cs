using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class CouponService : BaseService, ICouponService
    {

        public CouponService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public async Task<T?> AddEntityAsync<T, TEntity>(TEntity? entityDto) where TEntity : class
            where T : class
        {
            if (entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.POST, 
                $"{StaticDetails.CouponAPIBase}api/coupons/create", 
                entityDto);

            //SendAsync may return null
            var response = await SendAsync<T>(request);

            return response;
        }


        public async Task<T?> GetAllEntitiesAsync<T>() 
            where T : class
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
                $"{StaticDetails.CouponAPIBase}api/coupons/GetById/{id}");

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> RemoveEntityAsync<T>(int id) where T : class
        {
            if (id < 1) return null;

            var request = RequestDtoFactory.CreateRequestDto         (StaticDetails.ApiType.DELETE, 
                $"{StaticDetails.CouponAPIBase}api/coupons/remove/{id}");
                

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> UpdateEntityAsync<T, TEntity>(string id, TEntity? entityDto) 
            where T : class 
            where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(id) || entityDto is null) return null;

            var request = RequestDtoFactory.CreateRequestDto(StaticDetails.ApiType.PUT, 
                $"{StaticDetails.CouponAPIBase}api/coupons/update", 
                entityDto);
            
            var response = await SendAsync<T>(request);

            return response;
        }
    }
}
