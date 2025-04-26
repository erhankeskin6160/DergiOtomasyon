using AutoMapper;
using DergiOtomasyon.DTO;
using DergiOtomasyon.Models;

namespace DergiOtomasyon.AutoMapper
{
    public class AutoMapperConfig:Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Category, DergiDto>()
          .ForMember(dest => dest.CategoryName,
                     opt => opt.MapFrom(src => src.CategoryName))
          .ForMember(dest => dest.Count,
                     opt => opt.MapFrom(src => src.Magazine.SelectMany(m=>m.MagazineInfo).Count()));
        }
    }
}
