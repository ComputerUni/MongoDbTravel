using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.ReservationDtos;

namespace Travel.Web.Areas.User.ViewComponents.ProfileViewComponents
{
    public class _ProfileScriptViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
