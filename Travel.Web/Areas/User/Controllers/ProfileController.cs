using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;
using Travel.Web.DTOs.ProfileDtos;
using Travel.Web.Entities;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ProfileController(UserManager<AppUser> _userManager, IReservationService _reservationService, IFavoriteService _favoriteService, ICommentService _commentService, IQuestionService _questionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var reservations = await _reservationService.GetByUserIdAsync(user.Id.ToString());
            var favorites = await _favoriteService.GetByUserIdAsync(user.Id.ToString());
            var comments = await _commentService.GetByUserIdAsync(user.Id.ToString());
            var questions = await _questionService.GetByUserIdAsync(user.Id.ToString());

            var viewModel = new ProfileViewModel
            {
                User = user,
                Reservations = reservations,
                Favorites = favorites,
                Comments = comments,
                Questions = questions
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.City = dto.City;
            user.Country = dto.Country;
            user.BirthDate = dto.BirthDate;

            await _userManager.UpdateAsync(user);
            return Ok();
        } 
    }
}
