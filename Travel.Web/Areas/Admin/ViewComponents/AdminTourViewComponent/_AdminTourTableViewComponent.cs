using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminTourViewComponent
{
    public class _AdminTourTableViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
