using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DashboardServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardPopularToursViewComponent(IDashboardService _dashboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var popularTours = await _dashboardService.GetPopularToursAsync();
            return View(popularTours);
        }
    }
}
