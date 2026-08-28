using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.Entities;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class QuestionController(IQuestionService _questionService, UserManager<AppUser> _userManager) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateQuestionDto createQuestionDto)
        {
            var user = await _userManager.GetUserAsync(User);
            createQuestionDto.UserId = user.Id.ToString();
            await _questionService.CreateAsync(createQuestionDto);
            return Ok();
        }
    }
}
