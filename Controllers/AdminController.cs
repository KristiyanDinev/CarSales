using CarSales.Models;
using CarSales.Models.Forms;
using CarSales.Models.Identity;
using CarSales.Models.Views.Admin;
using CarSales.Services;
using CarSales.Utilities;
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
        private static readonly string AdminRoleName = "Admin";
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
        [Route("/admin/cars")]
        public async Task<IActionResult> Cars([FromQuery] CarQueryParametersModel carQuery)
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

            return View(new AdminHomeViewModel
            {
                Cars = await _carService.GetCarsAsync(carQuery),
                User = currentUser,
                CurrentPage = carQuery.Page
            });
        }


        [HttpGet]
        [Route("/admin/users")]
        public async Task<IActionResult> Users([FromQuery] int Page = 1)
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

            List<UserViewAdminModel> usersView = new List<UserViewAdminModel>();

            foreach (IdentityUserModel user in 

                await Utility.GetPageAsync<IdentityUserModel>(_userManager.Users
                     .Where(u => u.Id != currentUser.Id), Page))
            {
                usersView.Add(new UserViewAdminModel
                {
                    User = user,
                    IsAdmin = await _userManager.IsInRoleAsync(user, AdminRoleName)
                });
            }

            return View(new AdminHomeViewModel
            {
                User = currentUser,
                Users = usersView,
                CurrentPage = Page
            });
        }


        [HttpPost]
        [Route("/admin/car/create")]
        public async Task<IActionResult> CreateCar([FromForm] CreateCarFormModel createCarForm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Model.";
                return BadRequest();
            }

            if (createCarForm.Image == null || createCarForm.Image.Length == 0)
            {
                TempData["Error"] = "Image is required.";
                return BadRequest();
            }

            if (!await _carService.CreateCarAsync(createCarForm))
            {
                TempData["Error"] = "Couldn't create that Car.";
                return BadRequest();
            }

            TempData["Success"] = "Created!";
            return Ok();
        }


        [HttpPost]
        [Route("/admin/car/edit")]
        public async Task<IActionResult> EditCar([FromForm] CreateCarFormModel createCarForm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Model.";
                return BadRequest();
            }

            if (createCarForm.Id == null)
            {
                TempData["Error"] = "Id is required.";
                return BadRequest();
            }

            if (!await _carService.EditCarAsync(createCarForm))
            {
                TempData["Error"] = "Couldn't edit that Car.";
                return BadRequest();
            }

            TempData["Success"] = "Edited!";
            return Ok();
        }


        [HttpPost]
        [Route("/admin/car/delete/{id}")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            if (!await _carService.DeleteCarAsync(id))
            {
                TempData["Error"] = "Couldn't delete Car.";
                return BadRequest();
            }

            TempData["SuccessS"] = "Deleted!";
            return Ok();
        }


        [HttpPost]
        [Route("/admin/user/role")]
        public async Task<IActionResult> ManagerRole([FromForm] UserRoleFormModel userAndRoleForm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid Data.";
                return BadRequest();
            }

            IdentityRoleModel? role = await _roleManager.FindByNameAsync(AdminRoleName);
            if (role == null)
            {
                TempData["Error"] = "Role not found.";
                return NotFound();
            }

            IdentityUserModel? user = await _userManager.FindByIdAsync(userAndRoleForm.UserId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return NotFound();
            }

            IdentityResult result;
            if (userAndRoleForm.IsAdmin)
            {
                result = await _userManager.AddToRoleAsync(user, AdminRoleName);

            } else
            {
                result = await _userManager.RemoveFromRoleAsync(user, AdminRoleName);
            }

            if (result.Succeeded)
            {
                TempData["Success"] = userAndRoleForm.IsAdmin ? 
                    $"Added Admin Role for {user.Email}" : 
                    $"Removed Admin Role for {user.Email}";
                return Ok();
            }

            TempData["Error"] = userAndRoleForm.IsAdmin ? 
                $"Couldn't add Admin Role for {user.Email}" : 
                $"Couldn't remove Admin Role for {user.Email}";
            return BadRequest();
        }
    }
}
