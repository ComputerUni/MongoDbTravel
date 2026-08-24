using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.DestinationDtos;
using Travel.Web.Services.DestinationServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DestinationController(IDestinationService _destinationService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var destinations = await _destinationService.GetAllAsync();
            return View(destinations);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDestinationDto createDestinationDto)
        {
            if(!ModelState.IsValid)
            {
                return View(createDestinationDto);
            }

            await _destinationService.CreateAsync(createDestinationDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var destination = await _destinationService.GetByIdAsync(id);
            var updateDestination = _mapper.Map<UpdateDestinationDto>(destination);

            updateDestination.ExistingImage = destination.ImageUrl;

            return View(updateDestination);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateDestinationDto updateDestinationDto)
        {
            if(!ModelState.IsValid)
            {
                return View(updateDestinationDto);
            }

            await _destinationService.UpdateAsync(updateDestinationDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _destinationService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
