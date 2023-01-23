using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok.Models;
using Pustok.ViewModels;

namespace Pustok.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(MemberRegisterVM memberRegisterVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;
            user =await _userManager.FindByNameAsync(memberRegisterVM.UserName);
            if(user != null)
            {
                ModelState.AddModelError("UserName", "Already exists");
                return View();
            }
            user= await _userManager.FindByEmailAsync(memberRegisterVM.Email);
            if(user != null)
            {
                ModelState.AddModelError("Email", "Already exists");
                return View();
            }
            user = new AppUser
            {
                UserName = memberRegisterVM.UserName,
                FullName = memberRegisterVM.FullName,
                Email = memberRegisterVM.Email
            };
            var result=await _userManager.CreateAsync(user, memberRegisterVM.Password);
            if (!result.Succeeded)
            {
                return View();
            }

             await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, isPersistent: false);




            return RedirectToAction("index", "home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginVm loginvm)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;
             user = await _userManager.FindByNameAsync(loginvm.Username);
            if(user== null)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginvm.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password incorrect");
                return View();
            }
            //if (!ModelState.IsValid) return View();

            return RedirectToAction("index", "home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }
    }
}
