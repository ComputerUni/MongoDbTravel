using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.AuthViewComponent
{
    public class _AuthHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
