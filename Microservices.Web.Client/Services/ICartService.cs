using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services
{
    public interface ICartService
    {
        Task<ResponseDto?> ApplyCouponCodeAsync(CartDto cartDto);
        Task<ResponseDto?> GetCartByIdAsync(string userId);
        Task<ResponseDto?> RemoveCartAsync(int cartDetailsId);
        Task<ResponseDto?> RemoveCouponAsync(string userId);
        Task<ResponseDto?> UpsertCartAsync(CartDto couponDto);
    }
}