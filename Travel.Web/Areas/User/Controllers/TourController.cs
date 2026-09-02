using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.TourServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class TourController(ITourService _tourService, ICategoryService _categoryService, ICommentService _commentService, IQuestionService _questionService, IDestinationService _destinationService, ITourLocalizationService _tourLocalizationService) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var tours = await _tourService.GetActiveToursForUserAsync();
            var categories = await _categoryService.GetAllAsync();
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";

            var localizedList = new List<ResultTourDto>();

            foreach(var tour in tours)
            {
                var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(tour.Id, langCode);
                string displayCountry = tour.Country;
                string displayCategory = tour.CategoryName;

                if (langCode == "en")
                {
                    var matchedCategory = categories.FirstOrDefault(c => c.CategoryName == tour.CategoryName);
                    if (matchedCategory != null && !string.IsNullOrEmpty(matchedCategory.CategoryNameEn))
                    {
                        displayCategory = matchedCategory.CategoryNameEn;
                    }
                }

                if (langCode == "en")
                {
                    if (!string.IsNullOrEmpty(tour.DestinationId))
                    {
                        var destination = await _destinationService.GetByIdAsync(tour.DestinationId);
                        if(destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                        {
                            displayCountry = destination.CountryEn;
                        }
                    }
                }

                localizedList.Add(new ResultTourDto
                {
                    Id = tour.Id,
                    Name = localization?.Name ?? tour.Name,
                    ShortDescription = localization?.ShortDescription ?? tour.ShortDescription,
                    Description = localization?.Description ?? tour.Description,
                    Route = localization?.Route ?? tour.Route,
                    TourType = localization?.Transport ?? tour.Transport,
                    Accommodation = localization?.Accommodation ?? tour.Accommodation,
                    Country = displayCountry,
                    DestinationName = displayCountry,
                    CategoryName = displayCategory,

                    CoverImage = tour.CoverImage,
                    Price = tour.Price,
                    Duration = tour.Duration,
                    IsFeatured = tour.IsFeatured,
                    IsBest = tour.IsBest,
                    IsNew = tour.IsNew,
                    AverageRating = tour.AverageRating,
                    ReviewCount = tour.ReviewCount,
                    IsActive = tour.IsActive

                });
            }

   
            return View(localizedList.ToPagedList(page, 6));
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
