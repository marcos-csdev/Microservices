using AutoMapper;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Models.Factories;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly MsDbContext _dbContext;
        private readonly UserManager<MSUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        private readonly IMapper _mapper;

        public AuthService(MsDbContext dbContext, UserManager<MSUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var userRetrieved = await _dbContext.MSUsers.FirstOrDefaultAsync(user => user.UserName == loginRequestDto.UserName);

            if (userRetrieved is null) return null;

            var isValid = await _userManager.CheckPasswordAsync(userRetrieved, loginRequestDto.Password);

            var jwtToken = _jwtTokenGenerator.GenerateToken(userRetrieved);

            var userDto = _mapper.Map<MSUserDto>(userRetrieved);

            var response = LoginResponseDtoFactory.Create(userDto, jwtToken);

            return response;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            var user = MSUserFactory.Create(
                registrationRequestDto.Email,
                registrationRequestDto.Email,
                registrationRequestDto.Email.ToUpper(), registrationRequestDto.Name,
                registrationRequestDto.Phone);

            var userCreated = await _userManager.CreateAsync(user, registrationRequestDto.Password);

            if (userCreated.Succeeded)
            {
                var userFound = await _dbContext.MSUsers.FirstOrDefaultAsync(user => user.Email == registrationRequestDto.Email);

                var userDto = _mapper.Map<MSUserDto>(userFound);

                return "";
            }
            else
            {
                return userCreated.Errors.FirstOrDefault()!.Description;
            }

        }
    }
}
