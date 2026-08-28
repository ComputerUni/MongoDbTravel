using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController(IReservationService _reservationService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var reservations = await _reservationService.GetAllAsync();
            return View(reservations);
        }


        public async Task<IActionResult> UpdateStatus([FromBody] UpdateReservationStatusDto updateReservationDto)
        {

                await _reservationService.UpdateStatusAsync(updateReservationDto.Id, updateReservationDto.Status);
                return Ok();
           
        }
    }
}
