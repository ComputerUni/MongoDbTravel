using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController(IQuestionService _questionService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var questions = await _questionService.GetAllAsync();
            return View(questions);
        }

        public async Task<IActionResult> GetQuestionById(string id)
        {
            var value = await _questionService.GetByIdAsync(id);
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> AnswerQuestion(string questionId, string answer)
        {
            await _questionService.AnswerAsync(questionId, answer);
            return Json(new { success = true });
        }
    }
}
