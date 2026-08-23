using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Travel.Web.DTOs.AccountDtos;
using Travel.Web.Entities;

namespace Travel.Web.Areas.User.Controllers
{
    [Area("User")]
    public class AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signManager) : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(registerDto);
            }

            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Şifreler birbiriyle uyumlu değil.");
                return View(registerDto);
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                UserName = registerDto.UserName
            };

            user.PhoneNumber = registerDto.PhoneNumber;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return View(registerDto);
            }

            await _userManager.AddToRoleAsync(user, "User");

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost] 
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if(user == null)
            {
                ModelState.AddModelError(string.Empty, "Bu mail sistemde kayıtlı değil");
                return View(loginDto);
            }

            var result = await _signManager.PasswordSignInAsync(user, loginDto.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email veya şifre hatalı");
                return View(loginDto);
            }

            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", "Tour", new { area = "Admin" });
            else
                return RedirectToAction("Index", "Home", new { area = "User" });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return RedirectToAction("Login");
        }


    }
}
