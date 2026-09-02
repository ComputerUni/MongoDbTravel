using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DestinationServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _DestinationsPageViewComponent(IDestinationService _destinationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var allDestinations = await _destinationService.GetAllAsync();
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var isEnglish = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase);

            var destinations = (await _destinationService.GetAllAsync()).Where(x => x.IsPopular).Take(4).Select(x =>
            {
                x.Country = isEnglish && !string.IsNullOrEmpty(x.CountryEn) ? x.CountryEn : x.Country;
                x.Name = isEnglish && !string.IsNullOrEmpty(x.NameEn) ? x.NameEn : x.Name;
                x.Description = isEnglish && !string.IsNullOrEmpty(x.DescriptionEn) ? x.DescriptionEn : x.Description;
                return x;

            }).ToList();
            return View(destinations);
        }
    }
}
