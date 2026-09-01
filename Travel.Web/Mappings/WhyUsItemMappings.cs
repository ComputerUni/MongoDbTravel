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
            CreateMap<UpdateWhyUsItemDto, WhyUsItem>();
            CreateMap<WhyUsItem, ResultWhyUsItemDto>().ReverseMap();
            CreateMap<ResultWhyUsItemDto, UpdateWhyUsItemDto>();
        }
    }
}
