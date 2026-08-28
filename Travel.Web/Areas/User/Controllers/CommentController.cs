using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.Entities;
using Travel.Web.Services.CommentServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class CommentController(ICommentService _commentService, UserManager<AppUser> _userManager) : Controller
    {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto createCommentDto)
        {
            var user = await _userManager.GetUserAsync(User);
            createCommentDto.UserId = user.Id.ToString();
            await _commentService.CreateAsync(createCommentDto);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentDto commentDto)
        {
            var user = await _userManager.GetUserAsync(User);
            await _commentService.DeleteAsync(user.Id.ToString(), commentDto.CommentId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRating([FromBody] UpdateRatingDto dto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            await _commentService.UpdateRatingAsync(dto.CommentId, dto.Rating);
            return Ok();
        }
    }
}
