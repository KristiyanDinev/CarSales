using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [Authorize(Roles = "Admin")]
    [IgnoreAntiforgeryToken]
    [ApiController]
    public class AdminController : Controller
    {


        [HttpGet]
        [Route("/admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
