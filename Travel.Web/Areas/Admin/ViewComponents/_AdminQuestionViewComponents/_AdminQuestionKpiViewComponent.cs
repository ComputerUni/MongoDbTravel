using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.QuestionServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminQuestionViewComponents
{
    public class _AdminQuestionKpiViewComponent(IQuestionService _questionService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _questionService.GetQuestionKpiAsync();
            return View(kpi);
        }
    }
}
