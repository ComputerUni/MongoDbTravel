using Microsoft.AspNetCore.Mvc;

namespace Travel.Web.Areas.Admin.ViewComponents.AdminTourViewComponent
{
    public class _AdminTourHeadViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
