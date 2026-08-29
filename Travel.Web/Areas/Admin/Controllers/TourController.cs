using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.Admin.Models;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Services.ReportServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourController(ITourService _tourService, ILookupService _lookupService, ICategoryService _categoryService, IDestinationService _destinationService, IReportService _reportService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllAsync();
            return View(tours);
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
                VisaInfos = await _lookupService.GetByTypeAsync(((int)LookupType.VisaInfo).ToString())
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
            await _tourService.CreateAsync(createTourViewModel.Tour);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var tour = await _tourService.GetByIdAsync(id, false);
            var updateTour = _mapper.Map<UpdateTourDto>(tour);

            updateTour.ExistingCoverImage = tour.CoverImage;
            updateTour.ExistingGallery = tour.Gallery;

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
