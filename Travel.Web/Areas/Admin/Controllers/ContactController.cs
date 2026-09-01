using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ContactServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContactController(IContactService _contactService) : Controller
    {
        public async Task<IActionResult> Index(int page = 1)
        {
            var values = await _contactService.GetAllAsync();
            return View(values.ToPagedList(page, 6));
        }

        public async Task<IActionResult> DeleteContact(string id)
        {
            await _contactService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> MarkAsRead(string id)
        {
            await _contactService.MarkAsReadAsync(id);
            return RedirectToAction("Index");
        }
    }
}
