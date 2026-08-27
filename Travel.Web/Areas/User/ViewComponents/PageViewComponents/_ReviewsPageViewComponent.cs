using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;

namespace Travel.Web.Areas.User.ViewComponents.PageViewComponents
{
    public class _ReviewsPageViewComponent(ICommentService _commentService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var comments = await _commentService.GetAllAsync();
            var last3Comments = comments.Take(3).ToList();
            return View(last3Comments);
        }
    }
}
