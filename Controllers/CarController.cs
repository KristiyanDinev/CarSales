using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CarSales.Controllers
{

    [Authorize]
    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    public class CarController : Controller
    {

        public CarController() { }


        [HttpGet]
        [Route("/cars")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [Route("/car/{id}")]
        public IActionResult Car(int id)
        {
            return View();
        }
    }
}
