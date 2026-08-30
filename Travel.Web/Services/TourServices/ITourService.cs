using Travel.Web.Areas.Admin.Models;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllAsync();
        Task<ResultTourDto> GetByIdAsync(string id, bool resolveNames = true);
        Task<List<ResultTourDto>> GetActiveToursForUserAsync();
        Task<ResultTourDto> GetActiveTourByIdForUserAsync(string id, bool resolveNames = true);
        Task CreateAsync(CreateTourDto createTourDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateTourDto updateTourDto);
        Task<TourKpiDto> GetTourKpiAsync();
    }
}
