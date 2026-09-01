using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.IWhyUsServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _WhyPageViewComponent(IWhyUsService _whyUsService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _whyUsService.GetAllAsync();
            return View(values);
        }
    }
}
