using AutoMapper;
using Microservices.ProductAPI.Models;
using Microservices.ProductAPI.Models.Dto;

namespace Microservices.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            config.CreateMap<ProductDto, ProductModel>().ReverseMap());

            return mappingConfig;
        }
    }
}
