using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.CommentServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController(ICommentService _commentService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var comments = await _commentService.GetAllAsync();
            return View(comments);
        }

        public async Task<IActionResult> Approve(string commentId)
        {
            await _commentService.ApproveAsync(commentId);
            return Ok();
        }

        public async Task<IActionResult> MarkAsSpam(string commentId)
        {
            await _commentService.MarkAsSpamAsync(commentId);
            return Ok();
        }


    }
}
