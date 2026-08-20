using Travel.Web.DTOs.LookupDtos;

namespace Travel.Web.Services.LookupServices
{
    public interface ILookupService
    {
        Task<List<ResultLookupDto>> GetAllAsync();
        Task<ResultLookupDto> GetByIdAsync(string id);
        Task CreateAsync(CreateLookupDto createLookupDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateLookupDto updateLookupDto);
        Task<List<ResultLookupDto>> GetByTypeAsync(string type);
    }
}
