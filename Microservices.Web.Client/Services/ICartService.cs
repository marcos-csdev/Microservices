using Microservices.Web.Client.Models;

namespace Microservices.Web.Client.Services
{
    public interface ICartService
    {
        Task<ResponseDto?> ApplyCouponCodeAsync(CartDto cartDto);
        Task<ResponseDto?> GetCartByIdAsync(string userId);
        Task<ResponseDto?> RemoveCartAsync(string cartDetailsId);
        Task<ResponseDto?> UpsertCartAsync(CartDto couponDto);
    }
}