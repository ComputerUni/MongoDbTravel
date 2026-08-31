using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController(IUserService _userService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> SetPassive(string id)
        {
            await _userService.SetPassiveAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet] 
        public async Task<IActionResult> SetActive(string id)
        {
            await _userService.SetActiveAsync(id);
            return RedirectToAction("Index");
        }
    }
}
