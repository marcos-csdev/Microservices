using AutoMapper;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;

namespace Microservices.AuthAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MSUserDto, MSUser>().ReverseMap();
        }
        
    }
}
