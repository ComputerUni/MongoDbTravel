using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.User.ViewComponents.ProfileViewComponents
{
    public class _ProfileFooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
