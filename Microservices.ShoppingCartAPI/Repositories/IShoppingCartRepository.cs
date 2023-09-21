﻿using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Microservices.ShoppingCartAPI.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<int> UpsertCartDetailsAsync(CartDto cartDto);
        Task<int> DelectProductAsync([FromBody] int cartDetailsId);
        Task<CartDto> GetCartAsync(string userId);
        Task<CartDetailsModel?> GetCartDetailsAsync(int productId, int cartHeaderId);
        Task<CartHeaderModel?> GetCartHeadersAsync(string userId);
        Task<int> UpsertCartAsync(CartDto cartDto);
        Task<int> UpdateCartCountAsync(CartDto cartDto, CartDetailsModel dbCartDetails);
    }
}