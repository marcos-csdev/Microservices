using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;

namespace Microservices.AuthAPI.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly MsDbContext _dbContext;
        private readonly IMapper _mapper;

        public UserRepository(IMapper mapper, MsDbContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<EntityState> DeleteUserAsync(string email)
        {
            var user = await GetDbUserByEmailAsync(email);

            if (user is null) return EntityState.Unchanged;

            var deletedUser = _dbContext.Users.Remove(user);

            await _dbContext.SaveChangesAsync();

            return deletedUser.State;

        }
        
        public async Task<MSUser?> GetDbUserByUserNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return null!;

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(c => c.UserName == userName);

            return user;
        }
        public async Task<MSUser?> GetDbUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null!;

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(c => c.Email == email);

            return user;
        }

        public async Task<MSUserDto> GetUserByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return null!;

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(c => c.Id == userId);

            if (user is null) return null!;

            var userDto = _mapper.Map<MSUserDto>(user);

            return userDto;
        }


    }
}
