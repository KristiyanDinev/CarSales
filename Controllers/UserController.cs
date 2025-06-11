using CarSales.Models.Forms;
using CarSales.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    public class UserController : Controller
    {
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
                TempData["Error"] = "Invalid registration data.";
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
                SignInResult loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, lockoutOnFailure: false);
                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    TempData["Error"] = "Unable to register.";
                    return Unauthorized();
                }

                TempData["Success"] = "Registration successful!";
                return Ok();
            }

            TempData["Error"] = string.Join(", ", result.Errors.Select(e => e.Description));
            return Unauthorized();
        }


        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromForm] LoginFormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid login data.";
                return BadRequest();
            }

            IdentityUserModel? user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) {
                TempData["Error"] = "No such user found.";
                return BadRequest();
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["Success"] = "Login successful!";
                return Ok();
            }

            TempData["Error"] = "Login failed.";
            return BadRequest();
        }


        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
