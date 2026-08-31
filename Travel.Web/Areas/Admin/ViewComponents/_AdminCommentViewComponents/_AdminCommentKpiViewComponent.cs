using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminCommentViewComponents
{
    public class _AdminCommentKpiViewComponent(ICommentService _commentService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _commentService.GetCommentKpiAsync();
            return View(kpi);
        }
    }
}
