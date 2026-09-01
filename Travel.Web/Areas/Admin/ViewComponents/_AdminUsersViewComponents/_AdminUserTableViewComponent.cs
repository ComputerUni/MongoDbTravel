using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.UserServices;
using X.PagedList.Extensions;

namespace Travel.Web.Areas.Admin.ViewComponents._AdminUsersViewComponents
{
    public class _AdminUserTableViewComponent(IUserService _userService) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int page = 1)
        {
            var users = await _userService.GetAllAsync();
            return View(users.ToPagedList(page,6));
        }
    }
}
