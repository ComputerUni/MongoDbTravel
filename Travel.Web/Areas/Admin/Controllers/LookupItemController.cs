using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.LookupDtos;
using Travel.Web.Services.LookupServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LookupItemController(ILookupService _lookupService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var lookups = await _lookupService.GetAllAsync();
            return View(lookups);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLookupDto createLookupDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createLookupDto);
            }

            await _lookupService.CreateAsync(createLookupDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var lookup = await _lookupService.GetByIdAsync(id);
            var updateLookup = _mapper.Map<UpdateLookupDto>(lookup);
            return View(updateLookup);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateLookupDto updateLookupDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateLookupDto);
            }

            await _lookupService.UpdateAsync(updateLookupDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _lookupService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
