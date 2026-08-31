using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DashboardServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DashboardController(IDashboardService _dashboardService) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetChartData(string range = "6m")
        {
            var data = await _dashboardService.GetMonthlyReservationsAsync(range);
            return Json(data);
        }
    }
}
