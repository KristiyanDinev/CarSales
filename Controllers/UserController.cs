using CarSales.Models.Forms;
using CarSales.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    public class UserController : Controller
    {
        private const string InvalidLoginAttempt = "Invalid login attempt.";
        private const string AdminControllerName = "Admin";
        private const string CarControllerName = "Car";
        private const string IndexPage = "Car";

        private readonly UserManager<IdentityUserModel> _userManager;
        private readonly SignInManager<IdentityUserModel> _signInManager;
        //private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(
            UserManager<IdentityUserModel> userManager,
            SignInManager<IdentityUserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("/register")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        [Route("/login")]
        public IActionResult Login()
        {
            return View();
        }
        

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromForm] RegisterFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUserModel user = new IdentityUserModel
            {
                UserName = model.UserName,
                Email = model.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Assign default role (optional)
                //await _userManager.AddToRoleAsync(user, "User");
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(IndexPage, CarControllerName);
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }


        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromForm] LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, InvalidLoginAttempt);
                return View(model);
            }

            IdentityUserModel? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user.UserName == null) {
                ModelState.AddModelError(string.Empty, InvalidLoginAttempt);
                return View(model);
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(
                        user, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    return RedirectToAction(IndexPage, AdminControllerName);

                }
                return RedirectToAction(IndexPage, CarControllerName);
            }

            ModelState.AddModelError(string.Empty, InvalidLoginAttempt);

            return View(model);
        }


        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
