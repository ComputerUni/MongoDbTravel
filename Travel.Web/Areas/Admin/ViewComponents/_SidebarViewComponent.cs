using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.ContactServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.ViewComponents
{
    public class _SidebarViewComponent(ITourService _tourService, IReservationService _reservationService, ICommentService _commentService, IQuestionService _questionService, IContactService _contactService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var tours = await _tourService.GetAllAsync();
            var reservations = await _reservationService.GetAllAsync();
            var comments = await _commentService.GetAllAsync();
            var questions = await _questionService.GetAllAsync();
            var contacts = await _contactService.GetAllAsync();


            ViewBag.tours = tours.Count();
            ViewBag.reservations = reservations.Count();
            ViewBag.comments = comments.Count();
            ViewBag.questions = questions.Count();
            ViewBag.contacts = contacts.Count();

            return View();
        }
    }
}
