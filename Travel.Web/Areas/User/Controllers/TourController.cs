using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.TourServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class TourController(ITourService _tourService, ICommentService _commentService, IQuestionService _questionService) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var tours = await _tourService.GetActiveToursForUserAsync();
            return View(tours.ToPagedList(page, 6));
        }

        public async Task<IActionResult> Detail(string id)
        {
            var result = await _tourService.GetActiveTourByIdForUserAsync(id);
            var comments = await _commentService.GetByTourIdAsync(id);
            var questions = await _questionService.GetByTourIdAsync(id);
            var model = new TourDetailViewModel
            {
                Tour = result,
                Comments = comments,
                Questions = questions
            };
            return View(model);
        }

    }
}
