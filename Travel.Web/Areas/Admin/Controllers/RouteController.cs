using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.RouteDtos;
using Travel.Web.Services.RouteServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteController(IRouteService _routeService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var routes = await _routeService.GetAllAsync();
            return View(routes);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteDto createRouteDto)
        {
            if(!ModelState.IsValid)
            {
                return View(createRouteDto);
            }

            await _routeService.CreateAsync(createRouteDto);
            return RedirectToAction(nameof(Index));
        }


    }
}
