using Travel.Web.Entities;

namespace Travel.Web.Services.UserServices
{
    public interface IUserService
    {
        Task<List<AppUser>> GetAllAsync();
        Task<AppUser> GetByIdAsync(string id);
        Task SetActiveAsync(string id);
        Task SetPassiveAsync(string id);
    }
}
