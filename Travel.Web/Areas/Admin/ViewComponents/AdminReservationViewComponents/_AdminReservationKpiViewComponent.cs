using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.ReservationServices;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminReservationViewComponents
{
    public class _AdminReservationKpiViewComponent(IReservationService _reservationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _reservationService.GetReservationKpiAsync();
            return View(kpi);
        }
    }
}
