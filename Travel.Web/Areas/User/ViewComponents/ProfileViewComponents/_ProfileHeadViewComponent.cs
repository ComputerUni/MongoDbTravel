using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.ProfileViewComponents
{
    public class _ProfileHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
