using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.QuestionServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminQuestionViewComponents
{
    public class _AdminQuestionTableViewComponent(IQuestionService _questionService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var questions = await _questionService.GetAllAsync();
            return View(questions.ToPagedList(page, 6));
        }
    }
}
