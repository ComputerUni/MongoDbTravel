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
                await _reservationService.CreateAsync(dto);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
