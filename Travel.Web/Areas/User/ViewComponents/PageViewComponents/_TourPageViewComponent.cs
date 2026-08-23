using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _TourPageViewComponent(ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tours = (await _tourService.GetAllAsync()).Where(x => x.IsFeatured).Take(6).ToList();
            return View(tours);
        }
    }
}
