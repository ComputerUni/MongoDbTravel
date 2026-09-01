using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminTourViewComponent
{
    public class _AdminTourTableViewComponent(ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var tours = await _tourService.GetAllAsync();
            return View(tours.ToPagedList(page,6));
        }
    }
}
