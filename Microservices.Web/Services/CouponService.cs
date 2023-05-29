using Microservices.Web.Models;
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
            if (entityDto is null) return null!;

            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = entityDto,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupons",
                Token = ""
            };
            //SendAsync may return null
            var response = await SendAsync<T>(request);

            return response;
        }


        public async Task<T?> GetAllEntitiesAsync<T>() where T : class
        {
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.CouponAPIBase}api/coupons",
                Token = ""
            };

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> GetEntityByIdAsync<T>(int id) where T : class
        {
            if (id < 1) return null!;

            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupons/{id}",
                Token = ""
            };

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> RemoveEntityAsync<T>(int id) where T : class
        {
            if (id < 1) return null!;
            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupons/{id}",
                Token = ""
            };

            var response = await SendAsync<T>(request);

            return response;
        }

        public async Task<T?> UpdateEntityAsync<T>(string id, T entityDto) where T : class
        {
            if (string.IsNullOrWhiteSpace(id) || entityDto is null) return null!;

            var request = new RequestDto
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = entityDto,
                Url = $"{StaticDetails.CouponAPIBase}/api/coupons{id}",
                Token = ""
            };

            var response = await SendAsync<T>(request);

            return response;
        }
    }
}
