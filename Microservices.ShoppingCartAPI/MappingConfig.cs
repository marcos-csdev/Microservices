using AutoMapper;
using Microservices.ShoppingCartAPI.Models;
using Microservices.ShoppingCartAPI.Models.Dto;

namespace Microservices.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartDetailsDto, CartDetailsModel>().ReverseMap();
                config.CreateMap<CartHeaderDto, CartHeaderModel>().ReverseMap();
                //config.CreateMap<CartDto, >().ReverseMap();
            }
                
            );

            return mappingConfig;
        }
    }
}
