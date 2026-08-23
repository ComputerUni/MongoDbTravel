using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.TourListViewComponents
{
    public class _TourListHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
