using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Services.LocalizationServices;
using Travel.Web.Services.ReservationServices;
using Travel.Web.Services.TourServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ReservationController(IReservationService _reservationService, ITourService _tourService, ITourLocalizationService _tourLocalizationService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                string rawId = await _reservationService.CreateAsync(dto);
                string formattedId = "TRV-" + rawId.Substring(0, Math.Min(8, rawId.Length)).ToUpper();

                var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
                var isEnglish = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase);
                var cultureInfo = new System.Globalization.CultureInfo(isEnglish ? "en-US" : "tr-TR");

                var tour = await _tourService.GetByIdAsync(dto.TourId, false);
                string displayName = tour?.Name;

                if(isEnglish)
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(dto.TourId, "en");
                    if (localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                       displayName = localization.Name;
                    }
                }

                return Json(new { success = true, reservationId = formattedId, tourName = displayName });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
