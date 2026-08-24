using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.Admin.Models;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Entities.Enums;
using Travel.Web.Services.CategoryServices;
using Travel.Web.Services.DestinationServices;
using Travel.Web.Services.LookupServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourController(ITourService _tourService, ILookupService _lookupService, ICategoryService _categoryService, IDestinationService _destinationService, IMapper _mapper) : Controller
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
                foreach (var (key, value) in ModelState)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"KEY: {key} | ERROR: {error.ErrorMessage}");
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


    }
}
