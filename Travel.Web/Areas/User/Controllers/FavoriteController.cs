using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.FavoriteDtos;
using Travel.Web.Entities;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class FavoriteController(IFavoriteService _favoriteService, ITourService _tourService ,UserManager<AppUser> _userManager, ITourLocalizationService _tourLocalizationService, IDestinationService _destinationService) : Controller
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
            if (user == null) return Ok(new List<string>());

            var favorites = await _favoriteService.GetByUserIdAsync(user.Id.ToString());

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var isEnglish = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
            var langCode = isEnglish ? "en" : "tr";
            var cultureInfo = new System.Globalization.CultureInfo(isEnglish ? "en-US" : "tr-TR");

            if(isEnglish)
            {
                foreach(var f in favorites)
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(f.TourId, "en");
                    if (localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                        f.TourName = localization.Name;
                    }

                    if(langCode == "en")
                    {
                        var tour = await _tourService.GetByIdAsync(f.TourId);
                        if(tour != null && !string.IsNullOrEmpty(tour.DestinationId))
                        {
                            var destination = await _destinationService.GetByIdAsync(tour.DestinationId);
                            if(destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                            {
                                f.Country = destination.CountryEn;
                            }
                        }
                    }
                }
            }

            return Ok(favorites);
        }

    }
}
