using AutoMapper;
using Azure;
using Microservices.ShoppingCartAPI.Data;
using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            await _dbContext.CartHeaders.AddAsync(mappedCartHeader);

            await _dbContext.SaveChangesAsync();

            return mappedCartHeader;
        }

        public async Task<int> CreateCartDetailsAsync(CartDto cartDto)
        {
            var mappedCartHeader = await CreateCartHeadersAsync(cartDto);

            if (mappedCartHeader == null) return 0;

            var firstCartDetails = cartDto.CartDetails!.First();
            firstCartDetails.CartHeaderId = mappedCartHeader.CartHeaderId;

            var mappedCartDetails = _mapper.Map<CartDetailsModel>(firstCartDetails);

            if (mappedCartDetails == null) return 0;
            mappedCartDetails.CartHeader = mappedCartHeader;

            //headers may exist while details may not and vice-versa
            await _dbContext.CartDetails.AddAsync(mappedCartDetails);

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
        public async Task<CartDetailsModel?> GetCartDetailsAsync(int cartDetailsId)
        {
            if (cartDetailsId < 1) return null!;

            //without the AsNoTracking function, EF returns a tracking multiple requests error as this would be a second transaction being tracked
            var dbCartDetails = await _dbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(cd => cd.CartDetailsId == cartDetailsId);

            return dbCartDetails;
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
            //this allows to relate the user to the correct cart even when the ids are invalid 
            var firstCartDetails = cartDto.CartDetails!.First();

            firstCartDetails.Count += dbCartDetails.Count;
            firstCartDetails.CartHeaderId = dbCartDetails.CartHeaderId;
            firstCartDetails.CartDetailsId = dbCartDetails.CartDetailsId;

            //updating a second entity (cartDto) which leads EF to (try to) TRACK a new transaction
            _dbContext.CartDetails.Update(_mapper.Map<CartDetailsModel>(firstCartDetails));

            var changes = await _dbContext.SaveChangesAsync();

            return changes;
        }

        public async Task<int> UpdateCouponCodeAsync(CartDto cartDto)
        {
            var dbCartHeader = await _dbContext.CartHeaders.FirstAsync(ch => ch.UserId == cartDto.CartHeader!.UserId);

            if (dbCartHeader == null) return 0;

            dbCartHeader.CouponCode = cartDto.CartHeader!.CouponCode;

            _dbContext.CartHeaders.Update(dbCartHeader);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveCouponCodeAsync(CartDto cartDto)
        {
            var dbCartHeader = await _dbContext.CartHeaders.FirstAsync(ch => ch.UserId == cartDto.CartHeader!.UserId);

            if (dbCartHeader == null) return 0;

            dbCartHeader.CouponCode = "";

            _dbContext.CartHeaders.Update(dbCartHeader);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpsertCartAsync(CartDto cartDto)
        {
            var dbCartHeader = await GetCartHeadersAsync(cartDto.CartHeader!.UserId!);

            //Create cart header and details
            if (dbCartHeader == null)
            {
                return await CreateCartDetailsAsync(cartDto);
            }
            else//check if details has same product
            {
                //this allows to relate the user to the correct cart even when the ids are invalid 
                var firstCartDetails = cartDto.CartDetails!.First();

                var dbCartDetails = await GetCartDetailsAsync(firstCartDetails.ProductId, cartDto.CartHeader.CartHeaderId);

                if (dbCartDetails == null)
                {
                    //create cart details
                    return await CreateCartDetailsAsync(cartDto);
                }
                else//update cart details from DB
                {
                    return await UpdateCartCountAsync(cartDto, dbCartDetails);
                }
            }
        }

        private async Task RemoveCartHeader(int cartHeaderId, int cartTotalItems)
        {
            if (cartTotalItems == 1)
            {
                var cartHeader = await _dbContext.CartHeaders.FirstOrDefaultAsync(user => user.CartHeaderId == cartHeaderId);

                if (cartHeader == null)
                    return;

                _dbContext.CartHeaders.Remove(cartHeader);
            }
        }

        public async Task<int> RemoveCartAsync(int cartDetailsId)
        {
            var dbCartDetails = await GetCartDetailsAsync(cartDetailsId);

            var cartTotalItems = _dbContext.CartDetails.
                Where(user => user.CartDetailsId == cartDetailsId)
                .Count();

            if (dbCartDetails == null) return 0;

            _dbContext.CartDetails.Remove(dbCartDetails);

            //if there's only one item for the user, remove the cart header along (last item being removed from the cart)
            await RemoveCartHeader(dbCartDetails.CartHeaderId, cartTotalItems);

            return await _dbContext.SaveChangesAsync();

        }

        public async Task<int> RemoveProductAsync(int cartDetailsId)
        {
            if (cartDetailsId < 1) return 0;

            var dbCartDetails = await _dbContext.CartDetails.FirstOrDefaultAsync(cart => cart.CartDetailsId == cartDetailsId);

            if (dbCartDetails == null) return 0;

            var deletedProduct = _dbContext.CartDetails.Remove(dbCartDetails);

            return await _dbContext.SaveChangesAsync();
        }
    }
}
