using CarSales.Models.Database;
using CarSales.Models.Identity;

namespace CarSales.Models.Views.Admin
{
    public class AdminHomeViewModel
    {
        public required IdentityUserModel User { get; set; }
        public required List<CarModel> Cars { get; set; } = new List<CarModel>();
    }
}
