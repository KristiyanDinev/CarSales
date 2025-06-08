using CarSales.Models.Forms;
using CarSales.Models.Identity;
using CarSales.Services;
using CarSales.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    public class UserController : Controller
    {
        private const string InvalidLoginAttempt = "Invalid login attempt.";

        private readonly UserManager<IdentityUserModel> _userManager;
        private readonly SignInManager<IdentityUserModel> _signInManager;

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
        [Route("/")]
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
                return Unauthorized();
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
                return Ok();
            }

            ModelStateDictionary ModelState1 = ModelState;
            ErrorUtility.HandleModelErrors(result.Errors.Select(e => e.Description), ref ModelState1);

            return Unauthorized();
        }


        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromForm] LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized();
            }

            string[] errors = new[] { InvalidLoginAttempt };
            ModelStateDictionary ModelState1 = ModelState;

            IdentityUserModel? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                ErrorUtility.HandleModelErrors(errors, ref ModelState1);
                return Unauthorized();
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (signInResult.Succeeded)
            {
                return Ok();
            }

            ErrorUtility.HandleModelErrors(errors, ref ModelState1);
            return Unauthorized();
        }


        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
