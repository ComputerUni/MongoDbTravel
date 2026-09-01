using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.CategoryDtos;
using Travel.Web.Services.CategoryServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController(ICategoryService _categoryService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var categories = await _categoryService.GetAllAsync();
            return View(categories.ToPagedList(page, 6));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createCategoryDto);
            }

            await _categoryService.CreateAsync(createCategoryDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            var updateCategory = _mapper.Map<UpdateCategoryDto>(category);
            return View(updateCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateCategoryDto);
            }

            await _categoryService.UpdateAsync(updateCategoryDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
