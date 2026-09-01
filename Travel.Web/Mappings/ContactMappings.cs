using AutoMapper;
using Travel.Web.DTOs.ContactDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class ContactMappings : Profile
    {
        public ContactMappings()
        {
            CreateMap<CreateContactDto, Contact>();
            CreateMap<Contact, ResultContactDto>().ReverseMap();
        }
    }
}
