using AutoMapper;
using Travel.Web.DTOs.RouteDtos;
using route= Travel.Web.Entities.Route;

namespace Travel.Web.Mappings
{
    public class RouteMappings : Profile
    {
        public RouteMappings()
        {
            CreateMap<CreateRouteDto, route>();
            CreateMap<UpdateRouteDto, route>();
            CreateMap<route, ResultRouteDto>();
            CreateMap<ResultRouteDto, UpdateRouteDto>();
        }
    }
}
