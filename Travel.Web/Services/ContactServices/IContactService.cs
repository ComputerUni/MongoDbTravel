using Travel.Web.DTOs.ContactDtos;

namespace Travel.Web.Services.ContactServices
{
    public interface IContactService
    {
        Task<List<ResultContactDto>> GetAllAsync();
        Task<ResultContactDto> GetByIdAsync(string id);
        Task CreateAsync(CreateContactDto createContactDto);
        Task DeleteAsync(string id);
        Task MarkAsReadAsync(string id);
    }
}
