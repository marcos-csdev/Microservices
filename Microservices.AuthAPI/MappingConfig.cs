using AutoMapper;
using Microservices.AuthAPI.Models;
using Microservices.AuthAPI.Models.Dto;

namespace Microservices.AuthAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            config.CreateMap<MSUserDto, MSUser>().ReverseMap());

            return mappingConfig;
        }
    }
}
