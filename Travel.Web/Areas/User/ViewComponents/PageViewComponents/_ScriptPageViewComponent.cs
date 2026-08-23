using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _ScriptPageViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
