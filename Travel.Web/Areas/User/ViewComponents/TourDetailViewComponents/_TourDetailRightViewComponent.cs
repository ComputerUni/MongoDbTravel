using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.TourDtos;

namespace Travel.Web.Areas.User.ViewComponents.TourDetailViewComponents
{
    public class _TourDetailRightViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ResultTourDto tour)
        {
            return View(tour);
        }
    }
}
