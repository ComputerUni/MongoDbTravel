using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminCommentViewComponents
{
    public class _AdminCommentTableViewComponent(ICommentService _commentService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments.ToPagedList(page, 6));
        }
    }
}
