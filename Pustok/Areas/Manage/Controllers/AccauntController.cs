using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Areas.ViewModels;
using Pustok.Models;

namespace Pustok.Areas.Manage.Controllers
{
    [Area("manage")]
    public class AccauntController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccauntController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
           _userManager = userManager;
            _signInManager = signInManager;
        }

        

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel adminLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser admin = await _userManager.FindByNameAsync(adminLoginVM.Username);
            if(admin == null)
            {
                ModelState.AddModelError("", "Username or aa Password is incorrect");
                return View();
            }
            var result=await _signInManager.PasswordSignInAsync(admin, adminLoginVM.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or b Password is incorrect");
                return View();
            }


            return RedirectToAction("Index","dashboard");
        }
    }
}
