using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class TourController(ITourService _tourService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetActiveToursForUserAsync();
            return View(tours);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var result = await _tourService.GetActiveTourByIdForUserAsync(id);
            return View(result);
        }

    }
}
