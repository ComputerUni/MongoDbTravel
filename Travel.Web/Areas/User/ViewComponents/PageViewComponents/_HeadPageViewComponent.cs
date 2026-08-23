using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _HeadPageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
