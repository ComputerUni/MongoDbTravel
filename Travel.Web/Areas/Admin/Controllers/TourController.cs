using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourController(ITourService _tourService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourService.GetAllAsync();
            return View(tours);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTourDto createTourDto)
        {
            if(!ModelState.IsValid)
            {
                return View(createTourDto);
            }

            await _tourService.CreateAsync(createTourDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var tour = await _tourService.GetByIdAsync(id);
            var updateTour = _mapper.Map<UpdateTourDto>(tour);
            return View(updateTour);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTourDto updateTourDto)
        {
            if(!ModelState.IsValid)
            {
                return View(updateTourDto);
            }

            await _tourService.UpdateAsync(updateTourDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _tourService.DeleteAsync(id);
            return RedirectToAction("Index");
        }


    }
}
