using AutoMapper;
using Azure;
using Microservices.ShoppingCartAPI.Data;
using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel;
using System.Reflection.PortableExecutable;

namespace Microservices.ShoppingCartAPI.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly MsDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShoppingCartRepository(MsDbContext context, IMapper mapper)
        {
            _dbContext = context;
            _mapper = mapper;
        }

        public async Task<List<ProductDto>?> GetCartAsync()
        {
            //var dbProducts = await _dbContext.CartHeaders.ToListAsync();

            //if (dbProducts is null) return new List<ProductDto>();

            //var productsDto = _mapper.Map<List<ProductDto>>(dbProducts);

            //return productsDto;

            throw new NotImplementedException();
        }

        public async Task<CartHeaderModel?> GetCartHeadersAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null!;

            var dbCartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);

            return dbCartHeader;
        }

        public async Task UpsertCartAsync(CartDto cartDto)
        {
            if (cartDto is null ||
                cartDto.CartHeader is null ||
                cartDto.CartDetails is null)
                return;

            var cartHeaderFromDb = await _dbContext.CartHeaders.AsNoTracking()
                    .FirstOrDefaultAsync(u => u.UserId == cartDto.CartHeader.UserId);


            if (cartHeaderFromDb is null)
            {
                //create header and details
                var mappedCartHeader = _mapper.Map<CartHeaderModel>(cartDto.CartHeader);
                _dbContext.CartHeaders.Add(mappedCartHeader);
                //await _dbContext.SaveChangesAsync();

                cartDto.CartDetails.First().CartHeaderId = mappedCartHeader.CartHeaderId;
                _dbContext.CartDetails.Add(_mapper.Map<CartDetailsModel>(cartDto.CartDetails.First()));

                await _dbContext.SaveChangesAsync();
            }
            else
            {
                //if header is not null
                //check if details has same product
                var cartDetailsFromDb = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(
                    u => u.ProductId == cartDto.CartDetails.First().ProductId &&
                    u.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                if (cartDetailsFromDb == null)
                {
                    //create cartdetails
                    cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                    _dbContext.CartDetails.Add(_mapper.Map<CartDetailsModel>(cartDto.CartDetails.First()));
                }
                else
                {
                    //update count in cart details
                    cartDto.CartDetails.First().Count += cartDetailsFromDb.Count;

                    cartDto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;

                    cartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                    _dbContext.CartDetails.Update(_mapper.Map<CartDetailsModel>(cartDto.CartDetails.First()));
                }

                await _dbContext.SaveChangesAsync();
            }

        }

        public async Task<bool> DelectProductAsync([FromBody] int cartDetailsId)
        {
            if (cartDetailsId < 1) return false;

            var dbCartDetails = await _dbContext.CartDetails.FirstOrDefaultAsync(cart => cart.CartDetailsId == cartDetailsId);

            if (dbCartDetails is null) return false;

            var deletedProduct = _dbContext.CartDetails.Remove(dbCartDetails);

            await _dbContext.SaveChangesAsync();

            //if product was deleted, return true
            return deletedProduct.State != EntityState.Unchanged;
        }

    }
}
