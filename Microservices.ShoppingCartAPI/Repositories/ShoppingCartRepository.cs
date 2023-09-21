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

        public async Task<CartDto> GetCartAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null!;

            var dbCartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(u => u.UserId == userId);

            if (dbCartHeader == null) return null!;

            var mappedCartHeader = _mapper.Map<CartHeaderDto>(dbCartHeader);

            if (mappedCartHeader == null) return null!;

            var dbCartDetails = _dbContext.CartDetails.Where(u => u.CartHeaderId == dbCartHeader.CartHeaderId);

            if (!dbCartDetails.Any()) return null!;

            var mappedCartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(dbCartDetails);

            if (!mappedCartDetails.Any()) return null!;

            var cartDto = new CartDto
            {
                CartHeader = mappedCartHeader,
                CartDetails = mappedCartDetails
            };

            return cartDto;

        }

        private async Task<CartHeaderModel> CreateCartHeadersAsync(CartDto cartDto)
        {

            var mappedCartHeader = _mapper.Map<CartHeaderModel>(cartDto.CartHeader);
            if (mappedCartHeader == null) return null!;

            //headers may exist while details may not and vice-versa
            if(mappedCartHeader.CartHeaderId == 0)
            {
                await _dbContext.CartHeaders.AddAsync(mappedCartHeader);
            }
            else
            {
                _dbContext.CartHeaders.Update(mappedCartHeader);
            }


            await _dbContext.SaveChangesAsync();

            return mappedCartHeader;
        }
        public async Task<int> UpsertCartDetailsAsync(CartDto cartDto)
        {
            var mappedCartHeader = await CreateCartHeadersAsync(cartDto);
            
            if(mappedCartHeader == null) return 0;

            var firstCartDetails = cartDto.CartDetails!.First();
            firstCartDetails.CartHeaderId = mappedCartHeader.CartHeaderId;

            var mappedCartDetails = _mapper.Map<CartDetailsModel>(firstCartDetails);

            if (mappedCartDetails == null) return 0;
            mappedCartDetails.CartHeader = mappedCartHeader;

            //headers may exist while details may not and vice-versa
            if (mappedCartDetails.CartDetailsId == 0)
            {
                await _dbContext.CartDetails.AddAsync(mappedCartDetails);
            }
            else
            {
                _dbContext.CartDetails.Update(mappedCartDetails);
            }

            var changes = await _dbContext.SaveChangesAsync();

            return changes;
        }


        public async Task<CartHeaderModel?> GetCartHeadersAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null!;

            //without the AsNoTracking function, EF returns a tracking multiple requests error as this would be a second transaction being tracked
            var dbCartHeader = await _dbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync(ch => ch.UserId == userId);

            return dbCartHeader;
        }

        public async Task<CartDetailsModel?> GetCartDetailsAsync(int productId, int cartHeaderId)
        {
            if (productId < 1) return null!;

            //without the AsNoTracking function, EF returns a tracking multiple requests error as this would be a second transaction being tracked
            var dbCartDetails = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(cd => cd.ProductId == productId && cd.CartHeaderId == cartHeaderId);

            return dbCartDetails;
        }

        public async Task<int> UpdateCartCountAsync(CartDto cartDto, CartDetailsModel dbCartDetails)
        {

            //update count in cart details
            var firstCartDetails = cartDto.CartDetails!.First();

            firstCartDetails.Count += dbCartDetails.Count;
            firstCartDetails.CartHeaderId = dbCartDetails.CartHeaderId;
            firstCartDetails.CartDetailsId = dbCartDetails.CartDetailsId;

            //updating a second entity (cartDto) which leads EF to (try to) TRACK a new transaction
            _dbContext.CartDetails.Update(_mapper.Map<CartDetailsModel>(firstCartDetails));

            var changes = await _dbContext.SaveChangesAsync();

            return changes;
        }

        public async Task<int> UpsertCartAsync(CartDto cartDto)
        {
            var dbCartHeader = await GetCartHeadersAsync(cartDto.CartHeader!.UserId!);

            //Create cart header and details
            if (dbCartHeader == null)
            {
                return await UpsertCartDetailsAsync(cartDto);
            }
            else//check if details has same product
            {
                //cartDetails will only have one entry here since the only way to add an item is through the product details page
                var firstCartDetails = cartDto.CartDetails!.First();

                var dbCartDetails = await GetCartDetailsAsync(firstCartDetails.ProductId, dbCartHeader.CartHeaderId);

                if (dbCartDetails == null)
                {
                    //create cart details
                    return await UpsertCartDetailsAsync(cartDto);
                }
                else//update cart details from DB
                {
                    return await UpdateCartCountAsync(cartDto, dbCartDetails);
                }
            }
        }

        public async Task<int> DelectProductAsync([FromBody] int cartDetailsId)
        {
            if (cartDetailsId < 1) return 0;

            var dbCartDetails = await _dbContext.CartDetails.FirstOrDefaultAsync(cart => cart.CartDetailsId == cartDetailsId);

            if (dbCartDetails == null) return 0;

            var deletedProduct = _dbContext.CartDetails.Remove(dbCartDetails);

            return await _dbContext.SaveChangesAsync();
        }
    }
}
