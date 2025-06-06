using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CarSales.Controllers
{

    [EnableRateLimiting("fixed")]
    [IgnoreAntiforgeryToken]
    public class CarController : Controller
    {

        public CarController() { }


    }
}
