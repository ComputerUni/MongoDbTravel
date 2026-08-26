using AutoMapper;
using Travel.Web.DTOs.FavoriteDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class FavoriteMappings : Profile
    {
        public FavoriteMappings()
        {
            CreateMap<CreateFavoriteDto, Favorite>();
            CreateMap<Favorite, ResultFavoriteDto>().ReverseMap();
        }
    }
}
