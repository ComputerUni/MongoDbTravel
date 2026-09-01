using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.BannerDtos;
using Travel.Web.DTOs.WhyUsItemDtos;
using Travel.Web.Services.IWhyUsServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WhyUsItemController(IWhyUsService _whyUsService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var whyUsItems = await _whyUsService.GetAllAsync();
            return View(whyUsItems);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWhyUsItemDto createWhyUsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createWhyUsDto);
            }

            await _whyUsService.CreateAsync(createWhyUsDto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var item = await _whyUsService.GetByIdWhyUsAsync(id);
            var updateItem = _mapper.Map<UpdateWhyUsItemDto>(item);
            return View(updateItem);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateWhyUsItemDto updateWhyUsDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateWhyUsDto);
            }

            await _whyUsService.UpdateAsync(updateWhyUsDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _whyUsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
