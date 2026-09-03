using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;
using Travel.Web.DTOs.ProfileDtos;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.FavoriteServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ProfileController(UserManager<AppUser> _userManager, ITourService _tourService, IDestinationService _destinationService, IReservationService _reservationService, IFavoriteService _favoriteService, ITourLocalizationService _tourLocalizationService, ICommentService _commentService, IQuestionService _questionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var reservations = await _reservationService.GetByUserIdAsync(user.Id.ToString());
            var favorites = await _favoriteService.GetByUserIdAsync(user.Id.ToString());
            var comments = await _commentService.GetByUserIdAsync(user.Id.ToString());
            var questions = await _questionService.GetByUserIdAsync(user.Id.ToString());

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var isEnglish = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
            var cultureInfo = new System.Globalization.CultureInfo(isEnglish ? "en-US" : "tr-TR");

            if(isEnglish)
            {
                foreach(var q in questions)
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(q.TourId, "en");
                    if(localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                        q.TourName = localization.Name;
                    }
                }

                foreach(var c in comments)
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(c.TourId, "en");
                    if (localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                        c.TourName = localization.Name;
                    }

                    var tour = await _tourService.GetByIdAsync(c.TourId, false);
                    if (tour != null)
                    {
                        var destination = await _destinationService.GetByIdAsync(tour.DestinationId);
                        if (destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                        {
                            c.Country = destination.CountryEn;
                        }
                    }
                }


                foreach(var f in favorites)
                {
                    var localizationFav = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(f.TourId, "en");
                    if(localizationFav != null && !string.IsNullOrEmpty(localizationFav.Name))
                    {
                        f.TourName = localizationFav.Name;
                    }

                    var tour = await _tourService.GetByIdAsync(f.TourId, false);
                    if(tour != null)
                    {
                        var destination = await _destinationService.GetByIdAsync(tour.DestinationId);
                        if(destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                        {
                            f.Country = destination.CountryEn;
                        }
                    }

                    var questionsLoc = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(f.TourId, "en");
                    if(questionsLoc != null && !string.IsNullOrEmpty(questionsLoc.Name))
                    {
                        f.TourName = questionsLoc.Name;
                    }
                }
            }

            var reservationDatas = new List<object>();

            foreach(var r in reservations)
            {
                string displayTourName = r.TourName;

                if(isEnglish && !string.IsNullOrEmpty(r.TourId))
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(r.TourId, "en");
                    if(localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                        displayTourName = localization.Name;
                    }
                }

                string statusText = r.Status switch
                {
                    ReservationStatus.Onaylandı => isEnglish ? "Confirmed" : "Onaylandi",
                    ReservationStatus.İptal => isEnglish ? "Cancelled" : "İptal Edildi",
                    ReservationStatus.Bekliyor => isEnglish ? "Pending" : "Bekliyor",
                    _ => r.Status.ToString()
                };

                reservationDatas.Add(new
                {
                    tourName = displayTourName,
                    destinationName = r.DestinationName,
                    date = r.TourDate.ToLocalTime().ToString("dd MMM yyyy", cultureInfo),
                    createdAt = r.CreatedAt.ToLocalTime().ToString("dd MMM yyyy", cultureInfo),
                    adultCount = r.AdultCount,
                    childCount = r.ChildCount,
                    id = "TRV-" + r.Id.Substring(0, 8).ToUpper(),
                    status = statusText,
                    coverImage = r.CoverImage,
                    tourDate = r.TourDate,
                    tourId = r.TourId,
                    tourIsActive = r.TourIsActive,
                    reservationId = r.Id
                });

            }

            ViewBag.ReservationDatas = reservationDatas;

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

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] CancelReservationDto dto)
        {
            await _reservationService.UpdateStatusAsync(dto.Id, ReservationStatus.İptal);
            return Ok();
        }
    }
}
