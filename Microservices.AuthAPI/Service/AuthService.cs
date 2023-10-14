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
        private readonly IUserManagerService _userManagerService;
        private readonly IRoleManagerService _roleManagerService;

        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        private readonly IMapper _mapper;

        public AuthService(IUserManagerService userManager, IRoleManagerService roleManager, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
        {
            _userManagerService = userManager;
            _roleManagerService = roleManager;
            _mapper = mapper;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public async Task<LoginResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            var userRetrieved = await _userRepository.GetDbUserByUserNameAsync(loginRequestDto.UserName);

            if (userRetrieved == null) return null;

            var isValid = await _userManagerService.CheckPasswordAsync(userRetrieved, loginRequestDto.Password);

            if (isValid == false) 
                return null;

            var roles = await _userManagerService.GetRolesAsync(userRetrieved);
            var jwtToken = _jwtTokenGenerator.GenerateToken(userRetrieved, roles);

            var userDto = _mapper.Map<MSUserDto>(userRetrieved);

            if(userDto == null) return null;

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

            var userCreated = await _userManagerService.CreateAsync(user, registrationRequestDto.Password);

            var roleResult = await _userManagerService.AddToRoleAsync(user, registrationRequestDto.Role);

            if (userCreated.Succeeded == false || roleResult.Succeeded == false)
            {
                var errors = new List<string>();
                errors.AddRange(userCreated.Errors.Select(err => err.Description));
                if(errors.Count == 0) 
                    errors.AddRange(roleResult.Errors.Select(err => err.Description));

                return string.Join("; ", errors);
            }

            return "";
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = await _userRepository.GetDbUserByEmailAsync(email);

            if (user == null) return false;

            //if role doesnt exist, create it
            if (!await _roleManagerService.RoleExistsAsync(roleName))
            {
                await _roleManagerService.CreateAsync(roleName);
            }

            await _userManagerService.AddToRoleAsync(user, roleName);

            return true;

        }

        public async Task<bool> RemoveUser(string email)
        {
            var userFound = await _userRepository.GetDbUserByEmailAsync(email);

            if (userFound == null) return false;

            var isUserDeleted = await _userRepository.DeleteUserAsync(email);

            return isUserDeleted;
        }
    }
}
