using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class TourController(ITourService _tourService, ICommentService _commentService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetActiveToursForUserAsync();
            return View(tours);
        }

        public async Task<IActionResult> Detail(string id)
        {
            var result = await _tourService.GetActiveTourByIdForUserAsync(id);
            var comments = await _commentService.GetByTourIdAsync(id);
            var model = new TourDetailViewModel
            {
                Tour = result,
                Comments = comments
            };
            return View(model);
        }

    }
}
