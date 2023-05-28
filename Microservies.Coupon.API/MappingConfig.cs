using AutoMapper;
using Microservices.CouponAPI.Models;
using Microservices.CouponAPI.Models.Dto;

namespace Microservices.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            config.CreateMap<CouponDto, CouponModel>().ReverseMap());

            return mappingConfig;
        }
    }
}
