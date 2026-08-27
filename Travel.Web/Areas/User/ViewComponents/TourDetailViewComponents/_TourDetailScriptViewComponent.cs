using Microsoft.AspNetCore.Mvc;
using Travel.Web.Areas.User.Models;

namespace Travel.Web.Areas.User.ViewComponents.TourDetailViewComponents
{
    public class _TourDetailScriptViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TourDetailViewModel model)
        {
            return View(model);
        }
    }
}
