using AutoMapper;
using Travel.Web.DTOs.LookupDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class LookupItemMappings : Profile
    {
        public LookupItemMappings()
        {
            CreateMap<CreateLookupDto, LookupItem>();
            CreateMap<UpdateLookupDto, LookupItem>();
            CreateMap<LookupItem, ResultLookupDto>().ReverseMap();
            CreateMap<ResultLookupDto, UpdateLookupDto>();

        }
    }
}
