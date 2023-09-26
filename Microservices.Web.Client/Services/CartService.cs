using Microservices.Web.Client.Models;
using Microservices.Web.Client.Models.Factories;
using Microservices.Web.Client.Services.Abstractions;
using Microservices.Web.Client.Utility;

namespace Microservices.Web.Client.Services
{
    public class CartService : BaseService, ICartService
    {
        public CartService(IMessageService messageService) : base(messageService)
        {
        }

        public async Task<ResponseDto?> UpsertCartAsync(CartDto couponDto)
        {
            return await AddEntityAsync(couponDto, $"{StaticDetails.CartAPIUrl}/Upsert");

        }

        public async Task<ResponseDto?> GetCartByIdAsync(string userId)
        {
            return await GetEntityByIdAsync(
                $"{StaticDetails.CartAPIUrl}/GetCart/{userId}");
        }

        public async Task<ResponseDto?> RemoveCartAsync(string cartDetailsId)
        {
            return await RemoveEntityAsync(
                $"{StaticDetails.CartAPIUrl}/RemoveCart/{cartDetailsId}");
        }

        public async Task<ResponseDto?> ApplyCouponCodeAsync(CartDto cartDto)
        {
            return await UpdateEntityAsync(cartDto,
                $"{StaticDetails.CartAPIUrl}/ApplyCouponCode/");
        }

    }
}
