using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.DTOs.DestinationDtos;

namespace Travel.Web.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<List<ResultCategoryDto>> GetAllAsync();
        Task<ResultCategoryDto> GetByIdAsync(string id);
        Task CreateAsync(CreateCategoryDto createCategoryDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateCategoryDto updateCategoryDto);
    }
}
