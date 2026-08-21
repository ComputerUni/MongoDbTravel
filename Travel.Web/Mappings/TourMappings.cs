using AutoMapper;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class TourMappings : Profile
    {
        public TourMappings()
        {
            CreateMap<CreateTourDto, Tour>().ForMember(dest => dest.CoverImage, opt => opt.Ignore()).ForMember(dest => dest.Gallery, opt => opt.Ignore());
            CreateMap<UpdateTourDto, Tour>().ForMember(dest => dest.CoverImage, opt => opt.Ignore()).ForMember(dest => dest.Gallery, opt => opt.Ignore());
            CreateMap<Tour, ResultTourDto>().ReverseMap();
            CreateMap<ResultTourDto, UpdateTourDto>().ReverseMap().ForMember(dest => dest.CoverImage, opt => opt.Ignore()).ForMember(dest => dest.Gallery, opt => opt.Ignore());
            CreateMap<UpdateTourDto, ResultTourDto>().ReverseMap().ForMember(dest => dest.CoverImage, opt => opt.Ignore()).ForMember(dest => dest.Gallery, opt => opt.Ignore());
        }
    }
}
