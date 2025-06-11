using CarSales.Models.Database;
using CarSales.Models.Identity;

namespace CarSales.Models.Views.Admin
{
    public class AdminHomeViewModel
    {
        public required IdentityUserModel User { get; set; }
        public List<CarModel> Cars { get; set; } = new List<CarModel>();
        public List<UserViewAdminModel> Users { get; set; } = new List<UserViewAdminModel>();
        public required int CurrentPage { get; set; } = 1;
    }
}
