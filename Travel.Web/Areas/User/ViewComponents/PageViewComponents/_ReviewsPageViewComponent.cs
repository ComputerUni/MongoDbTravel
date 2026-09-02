using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;
using Travel.Web.Services.LocalizationServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _ReviewsPageViewComponent(ICommentService _commentService, ITourLocalizationService _tourLocalizationService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _commentService.GetAllAsync();
            var last3Comments = comments.Take(3).ToList();

            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;
            var langCode = currentCulture.StartsWith("en", StringComparison.OrdinalIgnoreCase) ? "en" : "tr";

            foreach (var comment in last3Comments)
            {
                if(!string.IsNullOrEmpty(comment.TourId))
                {
                    var localization = await _tourLocalizationService.GetLocalizationByTourAndLangAsync(comment.TourId, langCode);

                    if(localization != null && !string.IsNullOrEmpty(localization.Name))
                    {
                        comment.TourName = localization.Name;
                    }
                }
            }

            return View(last3Comments);
        }
    }
}
