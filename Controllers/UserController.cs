using CarSales.Models.Forms;
using CarSales.Models.Identity;
using CarSales.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.RateLimiting;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

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
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            ModelStateDictionary ModelState1 = ModelState;
            Utility.HandleModelErrors(result.Errors.Select(e => e.Description), ref ModelState1);

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
                Utility.HandleModelErrors(errors, ref ModelState1);
                return Unauthorized();
            }


            SignInResult res = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (res.Succeeded)
            {
                return Ok();
            }

            Utility.HandleModelErrors(errors, ref ModelState1);
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
