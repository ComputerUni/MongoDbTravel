using AutoMapper;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class DestinationMappings : Profile
    {
        public DestinationMappings()
        {
            CreateMap<CreateDestinationDto, Destination>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<UpdateDestinationDto, Destination>().ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            CreateMap<Destination, ResultDestinationDto>().ReverseMap();
            CreateMap<ResultDestinationDto, UpdateDestinationDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ExistingImage, opt => opt.Ignore());
        }
    }
}
