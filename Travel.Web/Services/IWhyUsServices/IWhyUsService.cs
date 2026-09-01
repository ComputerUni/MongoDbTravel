using Travel.Web.DTOs.WhyUsItemDtos;

namespace Travel.Web.Services.IWhyUsServices
{
    public interface IWhyUsService
    {
        Task<List<ResultWhyUsItemDto>> GetAllAsync();
        Task CreateAsync(CreateWhyUsItemDto createWhyUsDto);
        Task UpdateAsync(UpdateWhyUsItemDto updateWhyUsDto);
        Task DeleteAsync(string id);
        Task<UpdateWhyUsItemDto> GetByIdWhyUsAsync(string id);
    }
}
