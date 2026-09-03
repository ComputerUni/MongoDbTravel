using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.IWhyUsServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _WhyPageViewComponent(IWhyUsService _whyUsService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _whyUsService.GetAllAsync();

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase);

            if(langCode)
            {
                foreach(var item in values)
                {
                    item.Title = !string.IsNullOrEmpty(item.TitleEn) ? item.TitleEn : item.Title;
                    item.Description = !string.IsNullOrEmpty(item.DescriptionEn) ? item.DescriptionEn : item.Description;
                }
            }

            return View(values);
        }
    }
}
