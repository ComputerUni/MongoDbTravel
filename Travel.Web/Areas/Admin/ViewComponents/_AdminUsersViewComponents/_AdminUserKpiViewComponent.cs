using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.UserServices;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminUsersViewComponents
{
    public class _AdminUserKpiViewComponent(IUserService _userService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var kpi = await _userService.GetUserKpiAsync();
            return View(kpi);
        }
    }
}
