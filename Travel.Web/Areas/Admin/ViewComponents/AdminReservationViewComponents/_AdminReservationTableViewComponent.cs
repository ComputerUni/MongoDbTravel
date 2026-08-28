using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminReservationViewComponents
{
    public class _AdminReservationTableViewComponent(IReservationService _reservationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reservations = await _reservationService.GetAllAsync();
            return View(reservations);
        }
    }
}
