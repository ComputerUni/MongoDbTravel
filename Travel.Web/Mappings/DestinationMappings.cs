using AutoMapper;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class DestinationMappings : Profile
    {
        public DestinationMappings()
        {
            CreateMap<CreateDestinationDto, Destination>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.CountryEn, opt => opt.MapFrom(src => src.CountryEn))
                .ForMember(dest => dest.DescriptionEn, opt => opt.MapFrom(src => src.DescriptionEn));
            CreateMap<UpdateDestinationDto, Destination>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Destination, ResultDestinationDto>().ReverseMap();
            CreateMap<ResultDestinationDto, UpdateDestinationDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ExistingImage, opt => opt.Ignore());
        }
    }
}
