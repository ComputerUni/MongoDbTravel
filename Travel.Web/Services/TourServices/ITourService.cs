using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Services.TourServices
{
    public interface ITourService
    {
        Task<List<ResultTourDto>> GetAllAsync();
        Task<ResultTourDto> GetByIdAsync(string id);
        Task CreateAsync(CreateTourDto createTourDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateTourDto updateTourDto);
    }
}
