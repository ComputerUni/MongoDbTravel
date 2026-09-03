using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Travel.Web.Areas.User.Models;
using Travel.Web.DTOs.CommonDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Services.QuestionServices;
using Travel.Web.Services.TourServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class TourController(ITourService _tourService, ICategoryService _categoryService, ILookupService _lookupService, ICommentService _commentService, IQuestionService _questionService, IDestinationService _destinationService, ITourLocalizationService _tourLocalizationService) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var tours = await _tourService.GetActiveToursForUserAsync();

            var categories = await _categoryService.GetAllAsync();
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";

            var localizedList = new List<ResultTourDto>();

            foreach (var tour in tours)
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
                        if (destination != null && !string.IsNullOrEmpty(destination.CountryEn))
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
            var tour = await _tourService.GetActiveTourByIdForUserAsync(id);
            var comments = await _commentService.GetByTourIdAsync(id);
            var questions = await _questionService.GetByTourIdAsync(id);
            var lookupItems = await _lookupService.GetByTypeAsync("2");

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";

            string displayCountry = tour.Country;
            string displayCategory = tour.CategoryName;
            string displayDepartureCity = tour.DepartureCity;

            var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(tour.Id, langCode);

            var matchedCity = lookupItems.FirstOrDefault(c => c.Name == tour.DepartureCity);
            if(matchedCity != null && !string.IsNullOrEmpty(matchedCity.NameEn))
            {
                displayDepartureCity = matchedCity.NameEn;
            }

            if(langCode == "en")
            {
                foreach(var comment in comments)
                {
                    var commentTourLoc = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(comment.TourId, langCode);
                    if(commentTourLoc != null && !string.IsNullOrEmpty(commentTourLoc.Name))
                    {
                        comment.TourName = commentTourLoc.Name;
                    }
                }
            }

            if (langCode == "en")
            {
                var categories = await _categoryService.GetAllAsync();
                var matchedCategory = categories.FirstOrDefault(c => c.CategoryName == tour.CategoryName);
                if (matchedCategory != null && !string.IsNullOrEmpty(matchedCategory.CategoryNameEn))
                {
                    displayCategory = matchedCategory.CategoryNameEn;
                }

                if (!string.IsNullOrEmpty(tour.DestinationId))
                {
                    var destination = await _destinationService.GetByIdAsync(tour.DestinationId);
                    if (destination != null && !string.IsNullOrEmpty(tour.CountryEn))
                    {
                        displayCountry = destination.CountryEn;
                    }
                }
            }

            tour.Name = localization?.Name ?? tour.Name;
            tour.ShortDescription = localization?.ShortDescription ?? tour.ShortDescription;
            tour.Description = localization?.Description ?? tour.Description;
            tour.Route = localization?.Route ?? tour.Route;
            tour.TourType = localization?.Transport ?? tour.Transport;
            tour.Transport = localization?.Transport ?? tour.Transport;
            tour.Accommodation = langCode == "en" ? (localization?.Accommodation ?? tour.Accommodation) : tour.Accommodation; 
            tour.DepartureCity = displayDepartureCity;
            tour.GuideLanguage = langCode == "en" ? (localization?.GuideLanguage ?? tour.GuideLanguage) : tour.GuideLanguage;
            tour.VisaInfo = langCode == "en" ? (localization?.VisaInfo ?? tour.VisaInfo) : tour.VisaInfo;
            tour.MeetingPoint = langCode == "en" ? (localization?.MeetingPoint ?? tour.MeetingPoint) : tour.MeetingPoint; tour.Country = displayCountry;
            tour.DestinationName = displayCountry;
            tour.CategoryName = displayCategory;

            if(langCode == "en" && localization != null)
            {
                tour.Features = localization.Features;
                tour.Included = localization.Included;
                tour.NotIncluded = localization.NotIncluded;
                tour.DayPrograms = localization.DayPrograms.Select(dp => new DayProgramDto
                {
                    Title = dp.Title,
                    Description = dp.Description,
                    Accommodation = dp.Accommodation,
                    Transport = dp.Transport,
                    Meals = dp.Meals

                }).ToList();
            }

            var allTours = await _tourService.GetActiveToursForUserAsync();
            var similarTours = allTours.Where(x => x.Id != tour.Id).Take(3).ToList();

            if(langCode == "en")
            {
                foreach(var simTour in similarTours)
                {
                    var simLoc = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(simTour.Id, langCode);
                    if(simLoc != null)
                    {
                        simTour.Name = simLoc.Name;
                    }

                    if (!string.IsNullOrEmpty(simTour.DestinationId))
                    {
                        var destination = await _destinationService.GetByIdAsync(simTour.DestinationId);
                        if(destination != null && !string.IsNullOrEmpty(destination.CountryEn))
                        {
                            simTour.Country = destination.CountryEn;
                        }
                    }
                }
            }

            var model = new TourDetailViewModel
            {
                Tour = tour,
                Comments = comments,
                Questions = questions,
                SimilarTours = similarTours
            };

            return View(model);
        }

    }
}
