using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DashboardServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardRecentReservationsViewComponent(IDashboardService _dashboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var results = await _dashboardService.GetRecentReservationAsync();
            return View(results);
        }
    }
}
