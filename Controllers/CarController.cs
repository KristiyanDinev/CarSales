using CarSales.Models;
using CarSales.Models.Database;
using CarSales.Models.Identity;
using CarSales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;

namespace CarSales.Controllers
{

    [Authorize]
    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    public class CarController : Controller
    {
        private readonly UserManager<IdentityUserModel> _userManager;
        private readonly CarService _carService;

        public CarController(UserManager<IdentityUserModel> userManager, 
            CarService carService) {
            _userManager = userManager;
            _carService = carService;
        }


        [HttpGet]
        [Route("/cars")]
        public async Task<IActionResult> Index([FromQuery] CarQueryParametersModel carQuery)
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

            return View(new UserCarsModel
            {
                User = currentUser,
                Cars = await _carService.GetCarsAsync(carQuery)
            });
        }


        [HttpGet]
        [Route("/car/{id}")]
        public async Task<IActionResult> Car(int id)
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

            CarModel? car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            return View(new UserCarsModel
            {
                User = currentUser,
                Cars = new List<CarModel> { car }
            });
        }
    }
}
