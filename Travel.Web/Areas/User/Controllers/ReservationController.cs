using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class ReservationController(IReservationService _reservationService) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            dto.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try
            {
                string rawId = await _reservationService.CreateAsync(dto);

                string formattedId = "TRV-" + rawId.Substring(0, Math.Min(8, rawId.Length)).ToUpper();

                return Json(new { success = true, reservationId = formattedId });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
