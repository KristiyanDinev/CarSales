using CarSales.Enums;
using CarSales.Models;
using CarSales.Models.Forms;
using CarSales.Models.Identity;
using CarSales.Models.Views.Admin;
using CarSales.Services;
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
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUserModel> _userManager;
        private readonly RoleManager<IdentityRoleModel> _roleManager;
        private readonly CarService _carService;

        public AdminController(UserManager<IdentityUserModel> userManager, 
            RoleManager<IdentityRoleModel> roleManager, 
            CarService carService) {
            _userManager = userManager;
            _roleManager = roleManager;
            _carService = carService;
        }


        [HttpGet]
        [Route("/admin")]
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

            return View(new AdminHomeViewModel {
                Cars = await _carService.GetCarsAsync(new CarQueryParameters
                {
                    SortBy = CarSortEnum.CreatedAt,
                    SortDescending = true
                }),
                User = currentUser
            });
        }


        [HttpPost]
        [Route("/admin/car/create")]
        public async Task<IActionResult> CreateCar([FromForm] CreateCarFormModel createCarForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (createCarForm.Image == null || createCarForm.Image.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Image is required.");
                return BadRequest();
            }

            if (!await _carService.CreateCarAsync(createCarForm))
            {
                ModelState.AddModelError(string.Empty, "Couldn't create that Car.");
                return BadRequest();
            }

            TempData["SuccessStatus"] = "Created!";
            return Ok();
        }


        [HttpPost]
        [Route("/admin/car/edit")]
        public async Task<IActionResult> EditCar([FromForm] CreateCarFormModel createCarForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (createCarForm.Id == null)
            {
                ModelState.AddModelError(string.Empty, "Id is required.");
                return BadRequest();
            }

            if (!await _carService.EditCarAsync(createCarForm))
            {
                ModelState.AddModelError(string.Empty, "Couldn't edit that Car.");
                return BadRequest();
            }

            return Ok();
        }



        [HttpPost]
        [Route("/admin/user/role/add")]
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
        [Route("/admin/user/role/remove")]
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

    }
}
