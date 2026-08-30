using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DashboardServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminDashboardViewComponents
{
    public class _AdminDashboardKpiViewComponent(IDashboardService _dashboardService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _dashboardService.GetKpiAsync();
            return View(kpi);
        }
    }
}
