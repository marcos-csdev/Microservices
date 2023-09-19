using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ShoppingCartAPI.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<bool> DelectProductAsync([FromBody] int cartDetailsId);
        Task<CartDto> GetCartAsync(string userId);
        Task<CartHeaderModel?> GetCartHeadersAsync(string userId);
        Task UpsertCartAsync(CartDto cartDto);
    }
}