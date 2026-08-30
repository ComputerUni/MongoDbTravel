using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminTourViewComponent
{
    public class _AdminTourKpiViewComponent(ITourService _tourService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _tourService.GetTourKpiAsync();
            return View(kpi);
        }
    }
}
