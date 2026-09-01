using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminReservationViewComponents
{
    public class _AdminReservationTableViewComponent(IReservationService _reservationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var reservations = await _reservationService.GetAllAsync();
            return View(reservations.ToPagedList(page,6));
        }
    }
}
