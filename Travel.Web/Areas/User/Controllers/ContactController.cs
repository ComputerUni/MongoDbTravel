using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ContactDtos;
using Travel.Web.Services.ContactServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class ContactController(IContactService _contactService) : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateContactDto createContactDto)
        {
            createContactDto.SendDate = DateTime.Now;
            createContactDto.IsRead = false;
            await _contactService.CreateAsync(createContactDto);

            TempData["SuccessMessage"] = "Mesajınız başarıyla gönderildi!";
            return RedirectToAction("Index");
        }
    }
}
