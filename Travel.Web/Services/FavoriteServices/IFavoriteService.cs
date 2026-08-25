namespace Travel.Web.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        Task AddAsync(string userId, string tourId);
        Task RemoveAsync(string userId, string tourId);
        Task GetByUserIdAsync(string userId);
        Task IsFavoriteAsync(string userId, string tourId);
    }
}
