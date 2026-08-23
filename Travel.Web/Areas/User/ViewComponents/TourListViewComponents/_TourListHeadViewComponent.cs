using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.TourListViewComponents
{
    public class _TourListHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
