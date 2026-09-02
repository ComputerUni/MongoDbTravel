using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _TourPageViewComponent(ITourService _tourService, IDestinationService _destinationService ,ITourLocalizationService _tourLocalizationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allTours = await _tourService.GetAllAsync();
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";

            var featuredTours = allTours.Where(x => x.IsFeatured).Take(6).ToList();
            var resultList = new List<ResultTourDto>();

            foreach (var x in featuredTours)
            {
                var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(x.Id, langCode);

                string displayCountry = x.Country;
                if (langCode == "en" && !string.IsNullOrEmpty(x.DestinationId))
                {
                    var destination = await _destinationService.GetByIdAsync(x.DestinationId);
                    if (destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                    {
                        displayCountry = destination.CountryEn;
                    }
                }

                resultList.Add(new ResultTourDto
                {
                    Id = x.Id,
                    Name = localization?.Name ?? x.Name,
                    ShortDescription = localization?.ShortDescription ?? x.ShortDescription,
                    Route = localization?.Route ?? x.Route,
                    TourType = localization?.TourType ?? x.TourType,
                    Transport = localization?.Transport ?? x.Transport,
                    Accommodation = localization?.Accommodation ?? x.Accommodation,

                    CoverImage = x.CoverImage,
                    Price = x.Price,
                    Duration = x.Duration,
                    Country = displayCountry,
                    IsFeatured = x.IsFeatured,
                    IsBest = x.IsBest,
                    IsNew = x.IsNew,
                    IsActive = x.IsActive,
                    AverageRating = x.AverageRating,
                    ReviewCount = x.ReviewCount
                });
            }

            return View(resultList);
        }
    }
}
