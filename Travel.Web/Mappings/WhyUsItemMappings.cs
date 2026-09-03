using AutoMapper;
using Travel.Web.DTOs.WhyUsItemDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class WhyUsItemMappings : Profile
    {
        public WhyUsItemMappings()
        {
            CreateMap<CreateWhyUsItemDto, WhyUsItem>();
            CreateMap<UpdateWhyUsItemDto, WhyUsItem>()
                .ForMember(dest => dest.TitleEn, opt => opt.MapFrom(src => src.TitleEn))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn))
                .ReverseMap();
            CreateMap<WhyUsItem, ResultWhyUsItemDto>().ReverseMap();
            CreateMap<ResultWhyUsItemDto, UpdateWhyUsItemDto>();
        }
    }
}
