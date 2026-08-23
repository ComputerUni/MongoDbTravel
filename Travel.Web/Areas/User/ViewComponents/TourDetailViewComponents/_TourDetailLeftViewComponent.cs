using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.TourDetailViewComponents
{
    public class _TourDetailLeftViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
