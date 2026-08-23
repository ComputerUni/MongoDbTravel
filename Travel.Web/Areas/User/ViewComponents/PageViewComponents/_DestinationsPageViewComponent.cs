using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DestinationServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _DestinationsPageViewComponent(IDestinationService _destinationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinations = (await _destinationService.GetAllAsync()).Where(x => x.IsPopular).Take(4).ToList();
            return View(destinations);
        }
    }
}
