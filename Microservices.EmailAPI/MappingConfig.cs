using AutoMapper;
using Microservices.EmailAPI.Models;
using Microservices.EmailAPI.Models.Dto;

namespace Microservices.EmailAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            config.CreateMap<EmailLoggerDto, EmailLogger>().ReverseMap());

            return mappingConfig;
        }
    }
}
