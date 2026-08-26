using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.FavoriteDtos;
using Travel.Web.Entities;
using Travel.Web.Services.FavoriteServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class FavoriteController(IFavoriteService _favoriteService, UserManager<AppUser> _userManager) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateFavoriteDto favoriteDto)
        {
            var user = await _userManager.GetUserAsync(User);
            favoriteDto.UserId = user.Id.ToString();
            await _favoriteService.AddAsync(favoriteDto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteFavoriteDto favoriteDto)
        {
            var user = await _userManager.GetUserAsync(User);
            await _favoriteService.RemoveAsync(user.Id.ToString(), favoriteDto.TourId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> IsFavorite(string tourId)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _favoriteService.IsFavoriteAsync(user.Id.ToString(), tourId);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavorites()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _favoriteService.GetByUserIdAsync(user.Id.ToString());
            return Ok(favorites.Select(f => f.TourId));
        }

    }
}
