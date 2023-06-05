using AutoMapper;
using Microservices.AuthAPI.Data;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;
using Microservices.AuthAPI.Models.Factories;
using Microservices.AuthAPI.Repositories;
using Microservices.AuthAPI.Service.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Microservices.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<MSUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        private readonly IMapper _mapper;

        public AuthService(UserManager<MSUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var userRetrieved = await _userRepository.GetDbUserByUserNameAsync(loginRequestDto.UserName);

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
                var userFound = await _userRepository.GetDbUserByEmailAsync(registrationRequestDto.Email);

                var userDto = _mapper.Map<MSUserDto>(userFound);

                return "";
            }
            else
            {
                return userCreated.Errors.FirstOrDefault()!.Description;
            }

        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userRepository.GetDbUserByEmailAsync(email);

            if(user is null) return false;

            //if role doesnt exist, create it
            if(! await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await _userManager.AddToRoleAsync(user, roleName);

            return true;

        }

        public async Task<EntityState> RemoveUser(string email)
        {
            var userFound = _userRepository.GetDbUserByEmailAsync(email);

            if (userFound is null) return EntityState.Unchanged;

            var isUserDeleted = await _userRepository.DeleteUserAsync(email);



            return isUserDeleted;
        }
    }
}
