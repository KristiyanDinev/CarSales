using CarSales.Models.Forms;
using CarSales.Models.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [Authorize(Roles = "Admin")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    [Route("/admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUserModel> _userManager;
        private readonly RoleManager<IdentityRoleModel> _roleManager;

        public AdminController(UserManager<IdentityUserModel> userManager, 
            RoleManager<IdentityRoleModel> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            IdentityUserModel? currentUser = await _userManager.FindByIdAsync(userId);

            if (currentUser == null)
            {
                return Unauthorized();
            }

            return View();
        }


        [HttpPost]
        [Route("/user/role/add")]
        public async Task<IActionResult> AddRole([FromForm] UserRoleFormModel userAndRoleForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityRoleModel? role = await _roleManager.FindByNameAsync(userAndRoleForm.Role);
            if (role == null)
            {
                return BadRequest();
            }

            IdentityUserModel? user = await _userManager.FindByEmailAsync(userAndRoleForm.Email);
            if (user == null)
            {
                return BadRequest();
            }

            IdentityResult result = await _userManager.AddToRoleAsync(user, userAndRoleForm.Role);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/user/role/remove")]
        public async Task<IActionResult> RemoveRole([FromForm] UserRoleFormModel userAndRoleForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityRoleModel? role = await _roleManager.FindByNameAsync(userAndRoleForm.Role);
            if (role == null)
            {
                return BadRequest();
            }

            IdentityUserModel? user = await _userManager.FindByEmailAsync(userAndRoleForm.Email);
            if (user == null)
            {
                return BadRequest();
            }

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, userAndRoleForm.Role);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/role/create")]
        public async Task<IActionResult> CreateRole([FromForm] CreateRoleFormModel createRoleForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            IdentityResult result = await _roleManager.CreateAsync(
                new IdentityRoleModel
                {
                    Name = createRoleForm.Role,
                    Description = createRoleForm.Description ?? "No description provided."
                });

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }


        [HttpPost]
        [Route("/role/delete")]
        public async Task<IActionResult> DeleteRole([FromForm] string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return BadRequest();
            }

            IdentityRoleModel? role = await _roleManager.FindByNameAsync(Name);
            if (role == null)
            {
                return NotFound();
            }

            IdentityResult res = await _roleManager.DeleteAsync(role);
            if (res.Succeeded)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
