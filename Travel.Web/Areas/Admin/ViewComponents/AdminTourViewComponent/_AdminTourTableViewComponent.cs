using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminTourViewComponent
{
    public class _AdminTourTableViewComponent(ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tours = await _tourService.GetAllAsync();
            return View(tours);
        }
    }
}
