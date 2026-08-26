using Travel.Web.DTOs.FavoriteDtos;

namespace Travel.Web.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        Task AddAsync(CreateFavoriteDto dto);
        Task RemoveAsync(string userId, string tourId);
        Task<List<ResultFavoriteDto>> GetByUserIdAsync(string userId);
        Task<bool> IsFavoriteAsync(string userId, string tourId);
    }
}
