using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.TourDetailViewComponents
{
    public class _TourDetailHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
