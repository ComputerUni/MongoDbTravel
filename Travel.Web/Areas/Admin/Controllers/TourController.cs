using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.Admin.Models;
using Travel.Web.DTOs.CommonDtos;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Services.ReportServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class TourController(ITourService _tourService, ILookupService _lookupService, ICategoryService _categoryService, IDestinationService _destinationService, IReportService _reportService,ITourLocalizationService _tourLocalizationService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllAsync();
            var destinations = await _destinationService.GetAllAsync();
            var categories = await _categoryService.GetAllAsync();

            var viewModel = new TourListViewModel
            {
                Tours = tours,
                Destinations = destinations,
                Categories = categories
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new CreateTourViewModel
            {
                Tour = new CreateTourDto(),
                Categories = await _categoryService.GetAllAsync(),
                Destinations = await _destinationService.GetAllAsync(),
                TourTypes = await _lookupService.GetByTypeAsync(((int)LookupType.TourType).ToString()),
                Cities = await _lookupService.GetByTypeAsync(((int)LookupType.City).ToString()),
                Transports = await _lookupService.GetByTypeAsync(((int)LookupType.Transport).ToString()),
                GuideLanguages = await _lookupService.GetByTypeAsync(((int)LookupType.GuideLanguage).ToString()),
                VisaInfos = await _lookupService.GetByTypeAsync(((int)LookupType.VisaInfo).ToString()),
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTourViewModel createTourViewModel)
        {
            if (!ModelState.IsValid)
            {

                foreach (var kvp in ModelState)
                {
                    if (kvp.Value.Errors.Any())
                    {
                        Console.WriteLine($"Key: {kvp.Key}, Error: {kvp.Value.Errors[0].ErrorMessage}");
                    }
                }
                createTourViewModel.TourTypes = await _lookupService.GetByTypeAsync("TourType");
                createTourViewModel.Cities = await _lookupService.GetByTypeAsync("City");
                createTourViewModel.Transports = await _lookupService.GetByTypeAsync("Transport");
                createTourViewModel.GuideLanguages = await _lookupService.GetByTypeAsync("GuideLanguage");
                createTourViewModel.VisaInfos = await _lookupService.GetByTypeAsync("VisaInfo");
                createTourViewModel.Categories = await _categoryService.GetAllAsync();
                createTourViewModel.Destinations = await _destinationService.GetAllAsync();
                return View(createTourViewModel);
            }

            var tourId = await _tourService.CreateAndReturnIdAsync(createTourViewModel.Tour);

            var transportLookup = await _lookupService.GetByIdAsync(createTourViewModel.Tour.Transport);
            var guideLookup = await _lookupService.GetByIdAsync(createTourViewModel.Tour.GuideLanguage);
            var visaLookup = await _lookupService.GetByIdAsync(createTourViewModel.Tour.VisaInfo);
            var tourTypeLookup = await _lookupService.GetByIdAsync(createTourViewModel.Tour.TourType);

            var enLocalization = new TourLocalization
            {
                TourId = tourId,
                LanguageCode = "en",
                Name = createTourViewModel.Tour.NameEn,
                Description = createTourViewModel.Tour.DescriptionEn,
                ShortDescription = createTourViewModel.Tour.ShortDescriptionEn,
                Route = createTourViewModel.Tour.RouteEn,
                MeetingPoint = createTourViewModel.Tour.MeetingPointEn,
                Accommodation = createTourViewModel.Tour.AccommodationEn,
                Transport = transportLookup?.NameEn,
                GuideLanguage = guideLookup?.NameEn,
                VisaInfo = visaLookup?.NameEn,
                TourType = tourTypeLookup?.NameEn,
                Features = createTourViewModel.Tour.FeaturesEn ?? new(),
                Included = createTourViewModel.Tour.IncludedEn ?? new(),
                NotIncluded = createTourViewModel.Tour.NotIncludedEn ?? new(),
                DayPrograms = createTourViewModel.Tour.DayProgramsEn?.Select(x => new LocalizedDayProgram
                {
                    Title = x.Title,
                    Description = x.Description,
                     Accommodation = x.Accommodation,
                    Transport = x.Transport,         
                    Meals = x.Meals
                }).ToList() ?? new()
            };

            await _tourLocalizationService.SaveLocalizationAsync(enLocalization);



            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var tour = await _tourService.GetByIdAsync(id, false);
            var updateTour = _mapper.Map<UpdateTourDto>(tour);

            updateTour.ExistingCoverImage = tour.CoverImage;
            updateTour.ExistingGallery = tour.Gallery;

            var englishLocalization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(id, "en");

            if (englishLocalization != null)
            {
                updateTour.NameEn = englishLocalization.Name;
                updateTour.ShortDescriptionEn = englishLocalization.ShortDescription;
                updateTour.DescriptionEn = englishLocalization.Description;
                updateTour.RouteEn = englishLocalization.Route;
                updateTour.MeetingPointEn = englishLocalization.MeetingPoint;
                updateTour.TourTypeEn = englishLocalization.TourType;
                updateTour.TransportEn = englishLocalization.Transport;
                updateTour.AccommodationEn = englishLocalization.Accommodation;
                updateTour.GuideLanguageEn = englishLocalization.GuideLanguage;
                updateTour.VisaInfoEn = englishLocalization.VisaInfo;
                updateTour.IncludedEn = englishLocalization.Included;
                updateTour.NotIncludedEn = englishLocalization.NotIncluded;
                updateTour.FeaturesEn = englishLocalization.Features ?? new();
                updateTour.DayProgramsEn = englishLocalization.DayPrograms?.Select(dp => new DayProgramDto
                {
                    Title = dp.Title,
                    Description = dp.Description,
                    Accommodation = dp.Accommodation,
                    Transport = dp.Transport,
                    Meals = dp.Meals
                }).ToList();
            }

            var viewModel = new UpdateTourViewModel
            {
                Tour = updateTour,
                Categories = await _categoryService.GetAllAsync(),
                Destinations = await _destinationService.GetAllAsync(),
                TourTypes = await _lookupService.GetByTypeAsync(((int)LookupType.TourType).ToString()),
                Cities = await _lookupService.GetByTypeAsync(((int)LookupType.City).ToString()),
                Transports = await _lookupService.GetByTypeAsync(((int)LookupType.Transport).ToString()),
                GuideLanguages = await _lookupService.GetByTypeAsync(((int)LookupType.GuideLanguage).ToString()),
                VisaInfos = await _lookupService.GetByTypeAsync(((int)LookupType.VisaInfo).ToString())
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTourViewModel updateTourViewModel)
        {
            if (!ModelState.IsValid)
            {
                updateTourViewModel.TourTypes = await _lookupService.GetByTypeAsync("TourType");
                updateTourViewModel.Cities = await _lookupService.GetByTypeAsync("City");
                updateTourViewModel.Transports = await _lookupService.GetByTypeAsync("Transport");
                updateTourViewModel.GuideLanguages = await _lookupService.GetByTypeAsync("GuideLanguage");
                updateTourViewModel.VisaInfos = await _lookupService.GetByTypeAsync("VisaInfo");
                updateTourViewModel.Categories = await _categoryService.GetAllAsync();
                updateTourViewModel.Destinations = await _destinationService.GetAllAsync();

                return View(updateTourViewModel);
            }

            await _tourService.UpdateAsync(updateTourViewModel.Tour);

            var transportLookup = await _lookupService.GetByIdAsync(updateTourViewModel.Tour.Transport);
            var guideLookup = await _lookupService.GetByIdAsync(updateTourViewModel.Tour.GuideLanguage);
            var visaLookup = await _lookupService.GetByIdAsync(updateTourViewModel.Tour.VisaInfo);
            var tourTypeLookup = await _lookupService.GetByIdAsync(updateTourViewModel.Tour.TourType);

            var enLocalization = new TourLocalization
            {
                TourId = updateTourViewModel.Tour.Id,
                LanguageCode = "en",
                Name = updateTourViewModel.Tour.NameEn,
                Description = updateTourViewModel.Tour.DescriptionEn,
                ShortDescription = updateTourViewModel.Tour.ShortDescriptionEn,
                Route = updateTourViewModel.Tour.RouteEn,
                MeetingPoint = updateTourViewModel.Tour.MeetingPointEn,
                Accommodation = updateTourViewModel.Tour.AccommodationEn,
                Transport = transportLookup?.NameEn,
                GuideLanguage = guideLookup?.NameEn,
                VisaInfo = visaLookup?.NameEn,
                TourType = tourTypeLookup?.NameEn,
                Features = updateTourViewModel.Tour.FeaturesEn,
                Included = updateTourViewModel.Tour.IncludedEn ?? new(),
                NotIncluded = updateTourViewModel.Tour.NotIncludedEn ?? new(),
                DayPrograms = updateTourViewModel.Tour.DayProgramsEn?.Select(x => new LocalizedDayProgram
                {
                    DayNumber = x.DayNumber,
                    Title = x.Title,
                    Description = x.Description,
                    Accommodation = x.Accommodation,
                    Transport = x.Transport,
                    Meals = x.Meals
                }).ToList() ?? new()
            };

            await _tourLocalizationService.SaveLocalizationAsync(enLocalization);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _tourService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DownloadExcelReport(string tourId)
        {
            var stream = await _reportService.ExportTourReservationsToExcelAsync(tourId);
            string excelName = $"Tur_Katilimci_Raporu_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public async Task<IActionResult> DownloadPdfReport(string tourId)
        {
            var stream = await _reportService.ExportTourReservationsToPdfAsync(tourId);
            string fileName = $"Tur_Katilimci_Raporu_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(stream, "application/pdf", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadTourDateExcelReport(string tourId, string tourDateId, string status = null)
        {
            var stream = await _reportService.ExportTourDateReservationsToExcelAsync(tourId, tourDateId, status);
            string excelName = $"Tur_Katilimci_Raporu_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadTourDatePdfReport(string tourId, string tourDateId, string status = null)
        {
            var stream = await _reportService.ExportTourDateReservationsToPdfAsync(tourId, tourDateId, status);
            string fileName = $"Tur_Katilimci_Raporu_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return File(stream, "application/pdf", fileName);
        }

        [HttpGet]
        public async Task<IActionResult> GetTourDatesForReport(string tourId)
        {
            var tour = await _tourService.GetByIdAsync(tourId, resolveNames: false);
            var result = tour.Dates?.Select(d => new
            {
                id = d.Id,
                date = d.StartDate.ToLocalTime().ToString("dd.MM.yyyy")
            });

            return Json(result);
        }

    }
}
