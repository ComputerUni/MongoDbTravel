using Travel.Web.DTOs.RouteDtos;

namespace Travel.Web.Services.RouteServices
{
    public interface IRouteService
    {
        Task<List<ResultRouteDto>> GetAllByCityAsync(string city);
        Task<List<ResultRouteDto>> GetAllAsync();
        Task<ResultRouteDto> GetByIdAsync(string id);
        Task CreateAsync(CreateRouteDto createRouteDto);
        Task UpdateAsync(UpdateRouteDto updateRouteDto);
        Task DeleteAsync(string id);

    }
}
